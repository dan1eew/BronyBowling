using BronyBowling.Shared.Auth;
using BronyBowling.Shared.Data;
using BronyBowling.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

//builder.Services.AddCors(opt =>
//{
//    opt.AddDefaultPolicy(p =>
//        p.AllowAnyOrigin()
//         .AllowAnyHeader()
//         .AllowAnyMethod());
//});

var app = builder.Build();

// -------------------- MIDDLEWARE --------------------

app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// -------------------- ENDPOINTS --------------------

app.MapGet("/centers/{centerId}/lanes/available", async (
    int centerId,
    DateTime start,
    DateTime end,
    ApplicationDbContext db) =>
{
    var center = await db.BowlingCenters.FindAsync(centerId);
    if (center == null || !center.IsActive)
        return Results.BadRequest("Центр недоступен");

    var busyLanes = await db.Bookings
            .Where(b => b.CenterId == centerId
                    && b.Status != "Cancelled"
                    && start < b.EndTime
                    && end > b.StartTime)
            .Select(b => b.LaneNumber)
            .ToListAsync();

    var freeLanes = Enumerable.Range(1, center.LanesCount) // id != 1 или 2 в бд
            .Except(busyLanes)
            .ToList();

    return Results.Ok(freeLanes);
});

app.MapPost("/bookings", async (
    CreateBookingRequest request,
    ClaimsPrincipal user,
    ApplicationDbContext db) =>
{
    if (request.EndTime <= request.StartTime)
        return Results.BadRequest("Неправильны интервал времени");

    var center = await db.BowlingCenters.FindAsync(request.CenterId);
    if (center == null || !center.IsActive)
        return Results.BadRequest("Центр не работает");

    if (request.LaneNumber <= 0 || request.LaneNumber > center.LanesCount)
        return Results.BadRequest($"Введите номер дорожки с 1 - {center.LanesCount}");

    var hasConflict = await db.Bookings.AnyAsync(b =>
        b.CenterId == request.CenterId &&
        b.LaneNumber == request.LaneNumber &&
        b.Status != "Cancelled" &&
        request.StartTime < request.EndTime &&
        request.EndTime > b.StartTime
    );

    if (hasConflict)
        return Results.BadRequest("Дорожка занята");

    var bookingCode = Random.Shared.Next(0, 10000).ToString("D4");

    var booking = new Booking
    {
        CenterId = request.CenterId,
        LaneNumber = request.LaneNumber,
        StartTime = request.StartTime,
        EndTime = request.EndTime,
        BookingCode = bookingCode,
        CreatedAt = DateTime.UtcNow
    };

    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

    if (userId != null)
        booking.UserId = Guid.Parse(userId);
    else
    {
        if (string.IsNullOrEmpty(request.GuestName) ||
           string.IsNullOrEmpty(request.GuestPhone))
           return Results.BadRequest("Введите имя и телефон");

        booking.GuestName = request.GuestName;
        booking.GuestPhone = request.GuestPhone;
    }

    db.Bookings.Add(booking);
    await db.SaveChangesAsync();

    return Results.Ok(booking);
})
.AllowAnonymous();

app.MapGet("/centers", async (ApplicationDbContext db) =>
{
    var centers = await db.BowlingCenters
        .Where(c => c.IsActive)
        .Select(c => new CenterResponse(
            c.CenterId, c.Name, c.City, c.Street,
            c.House, c.Tariff!.WeekdayPrice, c.Tariff!.WeekendPrice
         ))
        .ToListAsync();

    return Results.Ok(centers);
});

app.Run("http://localhost:5280");
