using BronyBowling.Shared.Auth;
using BronyBowling.Shared.Data;
using BronyBowling.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using serviceBooking.API.DTOs;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
    var busyLaneIds = await db.Bookings
        .Where(b => b.Status != "Cancelled"
            && start < b.EndTime
            && end > b.StartTime)
        .Select(b => b.BowlingLaneId)
        .ToListAsync();

    var freeLanes = await db.BowlingLanes
        .Where(l => l.IsActive && !busyLaneIds.Contains(l.BowlingLaneId))
        .OrderBy(l => l.Number)
        .ToListAsync();

    return Results.Ok(freeLanes);
});

app.MapPost("/bookings", async (
    CreateBookingRequest request,
    ClaimsPrincipal user,
    ApplicationDbContext db) =>
{
    if (request.EndTime <= request.StartTime)
        return Results.BadRequest("Некорректный интервал");

    var hasConflict = await db.Bookings.AnyAsync(b =>
        b.BowlingLaneId == request.BowlingLaneId &&
        b.Status != "Cancelled" &&
        request.StartTime < b.EndTime &&
        request.EndTime > b.StartTime);

    if (hasConflict)
        return Results.BadRequest("Дорожка занята");

    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

    var booking = new Booking
    {
        BookingId = Guid.NewGuid(),
        BowlingLaneId = request.BowlingLaneId,
        StartTime = request.StartTime,
        EndTime = request.EndTime,
        CreatedAt = DateTime.UtcNow
    };

    if (0 >= request.BowlingLaneId || request.BowlingLaneId > 20)
        return Results.BadRequest("Введите корректный номер дорожки от 1 до 20");

    if (userId != null)
        booking.UserId = Guid.Parse(userId);
    else
    {
        if (string.IsNullOrWhiteSpace(request.GuestFullName) ||
            string.IsNullOrWhiteSpace(request.GuestPhone))
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
