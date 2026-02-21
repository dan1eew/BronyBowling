using BronyBowling.Shared.Auth;
using BronyBowling.Shared.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using serviceProfile.API.DTOs;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// ==================== SERVICES ====================

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

// CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy
            .WithOrigins(
                "http://localhost:5173",   
                "http://localhost:63230",   
                "http://localhost:5001"  
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

var app = builder.Build();

// ==================== MIDDLEWARE ====================

app.UseCors("Frontend");  

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
        LastName = entity.LastName
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

app.Run("http://localhost:5272");
