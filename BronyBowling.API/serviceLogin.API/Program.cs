using BronyBowling.Shared.Auth;
using BronyBowling.Shared.Data;
using BronyBowling.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using serviceLogin.API.DTOs;
using serviceLogin.API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
bool IsValidPhone(string phone) => 
     phone.Length == 11 && phone.All(char.IsDigit);

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

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p =>
      p.AllowAnyOrigin() .AllowAnyHeader() .AllowAnyMethod());
});

var app = builder.Build();

// -------------------- MIDDLEWARE --------------------

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
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
    if (string.IsNullOrWhiteSpace(request.PhoneNumber)
        || string.IsNullOrWhiteSpace(request.Password)
        || string.IsNullOrWhiteSpace(request.FirstName)
        || string.IsNullOrWhiteSpace(request.LastName))
        return Results.BadRequest("Заполните обязательные поля");

    if (await db.Users.AnyAsync(x => x.PhoneNumber == request.PhoneNumber))
        return Results.BadRequest("Пользователь уже существует");

    if (!IsValidPhone(request.PhoneNumber))
        return Results.BadRequest("Номер телефон должен содержать 11 цифр");

    var fullname = request.MiddleName is null
        ? $"{request.LastName} {request.FirstName}"
        : $"{request.LastName} {request.FirstName} {request.MiddleName}";

    var user = new User
    {
        UserId = Guid.NewGuid(),
        PhoneNumber = request.PhoneNumber,
        PasswordHash = hasher.Hash(request.Password),
        FirstName = request.FirstName,
        LastName = request.LastName,
        MiddleName = request.MiddleName,
        BirthDate = request.BirthDate,
        City = request.City,
        CreatedAt = DateTime.UtcNow
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Ok();
}); // register

app.MapPost("/login", async (
    LoginRequest request,
    ApplicationDbContext db,
    PasswordHasher hasher) =>
{
    var user = await db.Users
        .FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);

    if (user is null || !hasher.Verify(request.Password, user.PasswordHash))
        return Results.Unauthorized();

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Name, user.PhoneNumber)
    };

    var token = new JwtSecurityTokenHandler().WriteToken(
        new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials:
                new SigningCredentials(AuthOptions.SecurityKey, SecurityAlgorithms.HmacSha256)
        ));

    return Results.Ok(new { token });
}); // login

app.Run();
