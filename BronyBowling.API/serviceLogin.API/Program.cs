using BronyBowling.Shared.Auth;
using BronyBowling.Shared.Data;
using BronyBowling.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using serviceLogin.API.DTOs;
using serviceLogin.API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<PasswordHasher>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = AuthOptions.SecurityKey
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy
            .AllowAnyOrigin()          
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// -------------------- MIDDLEWARE --------------------

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

// -------------------- ENDPOINTS --------------------

app.MapPost("/register", async (
    RegisterRequest request,
    ApplicationDbContext db,
    PasswordHasher hasher) =>
{
    var validation = new ValidationResult
    {
        Errors = UserValidator.ValidateRegistration(
            request.PhoneNumber,
            request.Password,
            request.FirstName,
            request.LastName)
    };

    if (!validation.Success)
        return Results.BadRequest(validation);

    var exists = await db.Users
        .AnyAsync(x => x.PhoneNumber == request.PhoneNumber);

    if (exists)
        return Results.Conflict(new ValidationResult
        {
            Errors = { "Пользователь уже существует" }
        });

    var user = new User
    {
        UserId = Guid.NewGuid(),
        PhoneNumber = request.PhoneNumber!,
        PasswordHash = hasher.Hash(request.Password!),
        FirstName = request.FirstName!,
        LastName = request.LastName!,
        MiddleName = request.MiddleName,
        CreatedAt = DateTime.UtcNow
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapPost("/login", async (
    LoginRequest request,
    ApplicationDbContext db,
    PasswordHasher hasher) =>
{
    var user = await db.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);

    if (user == null || !hasher.Verify(request.Password, user.PasswordHash))
        return Results.Unauthorized();

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
    };

    var token = new JwtSecurityTokenHandler().WriteToken(
        new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(AuthOptions.SecurityKey, SecurityAlgorithms.HmacSha256)
        ));

    return Results.Ok(new { token });
});

app.Run("http://localhost:5001"); 