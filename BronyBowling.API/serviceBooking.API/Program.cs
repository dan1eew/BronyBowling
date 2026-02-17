using BronyBowling.Shared.Auth;
using BronyBowling.Shared.Data;
using BronyBowling.Shared.Models;
using BronyBowling.Shared.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NpgsqlTypes;
using serviceBooking.API.DTOs;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") )
);

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
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();

// -------------------- MIDDLEWARE --------------------

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

// -------------------- HELPERS --------------------
static DateTime EnsureUtc(DateTime dt) =>
    dt.Kind switch
    {
        DateTimeKind.Utc => dt,
        DateTimeKind.Local => dt.ToUniversalTime(),
        _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc)
    };

// -------------------- ENDPOINTS --------------------

app.MapGet("/lanes", async (ApplicationDbContext db) =>
{
    var lanes = await db.BowlingLanes
        .Where(x => x.IsActive)
        .OrderBy(x => x.Number)
        .ToListAsync();

    return Results.Ok(lanes);
});

app.MapGet("/lanes/available", async (
    DateTime start,
    DateTime end,
    ApplicationDbContext db) =>
{
    var lanes = await db.BowlingLanes
        .Where(x => x.IsActive)
        .OrderBy(x => x.Number)
        .ToListAsync();
    return Results.Ok(lanes);
});

app.MapPost("/bookings", async (
    CreateBookingRequest request,
    ClaimsPrincipal user,
    ApplicationDbContext db) =>
{
    var startUtc = EnsureUtc(request.StartTime);
    var endUtc = EnsureUtc(request.EndTime);

    // Проверка базовая
    var errors = BookingValidator.Validate(startUtc, endUtc, request.LaneId);
    if (errors.Any())
        return Results.BadRequest(errors);

    var newRange = new NpgsqlRange<DateTime>(startUtc, true, endUtc, false);

    // Проверка конфликта через PostgreSQL диапазон
    var hasConflict = await db.Bookings
        .Where(b => b.LaneId == request.LaneId && b.Status != "Cancelled")
        .AnyAsync(b => b.TimeRange.Overlaps(newRange));

    if (hasConflict)
        return Results.BadRequest("Дорожка занята в указанное время");

    var booking = new Booking
    {
        BookingId = Guid.NewGuid(),
        LaneId = request.LaneId,
        TimeRange = newRange,
        Status = "Created",
        CreatedAt = DateTime.UtcNow
    };

    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
    if (userId != null)
        booking.UserId = Guid.Parse(userId);
    else
    {
        if (string.IsNullOrWhiteSpace(request.GuestFullName) || string.IsNullOrWhiteSpace(request.GuestPhone))
            return Results.BadRequest("Введите ФИО и телефон");

        booking.GuestFullName = request.GuestFullName;
        booking.GuestPhone = request.GuestPhone;
    }

    db.Bookings.Add(booking);
    await db.SaveChangesAsync();

    return Results.Ok(booking);
})
.AllowAnonymous();

app.Run("http://localhost:5280");
