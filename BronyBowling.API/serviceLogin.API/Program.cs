using serviceLogin.API.Data;
using serviceLogin.API.DTOs;
using serviceLogin.API.Models;
using serviceLogin.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BronyBowling.Shared.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,

            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,

            ValidateLifetime = true,

            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };
    });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/register", async (
    RegisterRequest request,
    ApplicationDbContext db,
    PasswordHasher hasher) =>
{
    // базова€ валидаци€
    if (string.IsNullOrWhiteSpace(request.PhoneNumber)
        || string.IsNullOrWhiteSpace(request.Password)
        || string.IsNullOrWhiteSpace(request.FullName))
    {
        return Results.BadRequest("ќб€зательные пол€ не заполнены");
    }

    var exists = await db.Users
        .AnyAsync(u => u.PhoneNumber == request.PhoneNumber);

    if (exists)
        return Results.BadRequest("ѕользователь уже существует");

    var user = new User
    {
        UserId = Guid.NewGuid(),
        PhoneNumber = request.PhoneNumber,
        PasswordHash = hasher.Hash(request.Password),
        FullName = request.FullName,
        BirthDate = request.BirthDate, 
        City = request.City             
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Ok();
}); // Register

app.MapPost("/login", async (
    LoginRequest request,
    ApplicationDbContext db,
    PasswordHasher hasher) =>
{
    var user = await db.Users
        .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

    if (user is null)
        return Results.Unauthorized();

    if (!hasher.Verify(request.Password, user.PasswordHash))
        return Results.Unauthorized();

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Name, user.PhoneNumber)
    };

    var jwt = new JwtSecurityToken(
    issuer: AuthOptions.ISSUER,
    audience: AuthOptions.AUDIENCE,
    claims: claims,
    expires: DateTime.UtcNow.AddHours(1),
    signingCredentials: new SigningCredentials(
        AuthOptions.GetSymmetricSecurityKey(),
        SecurityAlgorithms.HmacSha256)
);

    var token = new JwtSecurityTokenHandler().WriteToken(jwt);

    return Results.Ok(new { token });
}); // Login

app.Run();