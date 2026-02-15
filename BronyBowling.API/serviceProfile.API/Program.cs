using BronyBowling.Shared.Auth;
using BronyBowling.Shared.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using serviceProfile.API.DTOs;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// ==================== SERVICES ====================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// AUTH (JWT)
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

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS — ОТКРЫТЫЙ (DEV)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

var app = builder.Build();

// ==================== MIDDLEWARE ====================


app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

// ==================== ENDPOINTS ====================

// ---------- GET PROFILE ----------
app.MapGet("/profile", async (
    ClaimsPrincipal user,
    ApplicationDbContext db) =>
{
    if (!user.Identity?.IsAuthenticated ?? true)
        return Results.Unauthorized();

    var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
    if (idClaim is null)
        return Results.Unauthorized();

    if (!Guid.TryParse(idClaim.Value, out var userId))
        return Results.Unauthorized();

    var entity = await db.Users.FindAsync(userId);
    if (entity is null)
        return Results.NotFound();

    return Results.Ok(new ProfileResponse
    {
        PhoneNumber = entity.PhoneNumber,
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        MiddleName = entity.MiddleName,
        BirthDate = entity.BirthDate,
        City = entity.City
    });
})
.RequireAuthorization();

// ---------- UPDATE PROFILE ----------
app.MapPut("/profile", async (
    UpdateProfileRequest request,
    ClaimsPrincipal user,
    ApplicationDbContext db) =>
{
    if (!user.Identity?.IsAuthenticated ?? true)
        return Results.Unauthorized();

    var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
    if (idClaim is null)
        return Results.Unauthorized();

    if (!Guid.TryParse(idClaim.Value, out var userId))
        return Results.Unauthorized();

    var entity = await db.Users.FindAsync(userId);
    if (entity is null)
        return Results.NotFound();

    if (string.IsNullOrWhiteSpace(request.FirstName) ||
        string.IsNullOrWhiteSpace(request.LastName))
        return Results.BadRequest("Имя и фамилия обязательны");

    entity.FirstName = request.FirstName;
    entity.LastName = request.LastName;
    entity.MiddleName = request.MiddleName;
    entity.BirthDate = request.BirthDate;
    entity.City = request.City;

    await db.SaveChangesAsync();
    return Results.Ok();
})
.RequireAuthorization();

// ---------- DELETE PROFILE ----------
app.MapDelete("/profile", async (
    ClaimsPrincipal user,
    ApplicationDbContext db) =>
{
    if (!user.Identity?.IsAuthenticated ?? true)
        return Results.Unauthorized();

    var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
    if (idClaim is null)
        return Results.Unauthorized();

    if (!Guid.TryParse(idClaim.Value, out var userId))
        return Results.Unauthorized();

    var entity = await db.Users.FindAsync(userId);
    if (entity is null)
        return Results.NotFound();

    db.Users.Remove(entity);
    await db.SaveChangesAsync();

    return Results.Ok("Профиль удалён");
})
.RequireAuthorization();

app.Run();
