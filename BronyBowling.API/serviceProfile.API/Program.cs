using BronyBowling.Shared.Auth;
using BronyBowling.Shared.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// CORS 
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("Frontend", policy =>
//        policy
//            .WithOrigins(
//                "http://localhost:5173",   
//                "http://localhost:63230",   
//                "http://localhost:5001"  
//            )
//            .AllowAnyHeader()
//            .AllowAnyMethod()
//    );
//});

var app = builder.Build();

// ==================== MIDDLEWARE ====================

app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Генерация схемы
    app.MapScalarApiReference(); // Подключение Scalar UI по умолчанию на /scalar/v1
}

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

app.MapGet("/profile/bookings", async (
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

    var bookings = await db.Bookings
        .Where(b => b.UserId == userId)
        .Select(b => new UserBookings(
            b.Center.Name, 
            b.LaneNumber,
            b.StartTime,
            b.EndTime,
            b.Status
            ))
        .OrderByDescending(b => b.StartTime)
        .ToListAsync();

    return Results.Ok(bookings);
})
.RequireAuthorization();

app.MapPost("/bookings/{id:int}/cancel", async (
    int id,
    ClaimsPrincipal user,
    ApplicationDbContext db) =>
{
    if (!user.Identity?.IsAuthenticated ?? true)
        return Results.Unauthorized();

    var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
    if (idClaim is null || !Guid.TryParse(idClaim.Value, out var userId))
        return Results.Unauthorized();

    var booking = await db.Bookings.FindAsync(id);
    if (booking is null)
        return Results.NotFound();

    // Проверка владельца
    if (booking.UserId != userId)
        return Results.Forbid();

    // Проверка статуса
    if (booking.Status == "Cancelled")
        return Results.BadRequest(new { error = "Бронь уже отменена" });

    if (booking.Status == "Paid")
        return Results.BadRequest(new { error = "Оплаченная бронь не может быть отменена" });

    // Проверка времени
    var minutesBeforeStart = (booking.StartTime - DateTime.UtcNow).TotalMinutes;

    if (minutesBeforeStart < 30)
        return Results.BadRequest(new
        {
            error = "Отмена возможна не позднее чем за 30 минут до начала"
        });

    booking.Status = "Cancelled";

    await db.SaveChangesAsync();

    return Results.Ok(new { message = "Бронь отменена" });
})
.RequireAuthorization();

app.Run("http://localhost:5272");
