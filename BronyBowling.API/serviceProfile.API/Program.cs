using serviceProfile.API.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BronyBowling.Shared.Auth;
using serviceLogin.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/profile", async (
    ClaimsPrincipal userClaims,
    ApplicationDbContext db) =>
{
    var userId = Guid.Parse(
        userClaims.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    var user = await db.Users.FindAsync(userId);

    if (user is null)
        return Results.NotFound();

    return Results.Ok(new ProfileResponse
    {
        PhoneNumber = user.PhoneNumber,
        FullName = user.FullName,
        BirthDate = user.BirthDate,
        City = user.City
    });
})
.RequireAuthorization();
// GET /profile

app.MapPut("/profile", async (
    UpdateProfileRequest request,
    ClaimsPrincipal userClaims,
    ApplicationDbContext db) =>
{
    var userId = Guid.Parse(
        userClaims.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    var user = await db.Users.FindAsync(userId);

    if (user is null)
        return Results.NotFound();

    user.FullName = request.FullName;

    // если пользователь ввёл — обновляем
    user.BirthDate = request.BirthDate;
    user.City = request.City;

    await db.SaveChangesAsync();

    return Results.Ok();
})
.RequireAuthorization();
// PUT /profile 

app.MapDelete("/profile", [Authorize] async (
    ClaimsPrincipal user,
    ApplicationDbContext db) =>
{
    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null)
        return Results.Unauthorized();

    int userId = int.Parse(userIdClaim.Value);

    var entity = await db.Users.FindAsync(userId);
    if (entity == null)
        return Results.NotFound();

    db.Users.Remove(entity);
    await db.SaveChangesAsync();

    return Results.Ok("Профиль удалён");
}); // DELETE /profile

app.Run();
