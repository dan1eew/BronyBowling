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

app.MapGet("/bookings", async (
    DateTime date,
    ApplicationDbContext db) =>
{
    var bookings = await db.Bookings
        .Where(b => b.StartTime.Date == date.Date && b.Status != "Cancelled")
        .ToListAsync();

    return Results.Ok(bookings);
});

app.MapPost("/bookings", async (
    CreateBookingRequest request,
    ClaimsPrincipal user,
    ApplicationDbContext db) =>
{
    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
    if (userId is null)
        return Results.Unauthorized();

    if (request.EndTime <= request.StartTime)
        return Results.BadRequest("Некорректный интервал времени");

    var hasConflict = await db.Bookings.AnyAsync(b =>
        b.BowlingLaneId == request.BowlingLaneId &&
        b.Status != "Cancelled" &&
        request.StartTime < b.EndTime &&
        request.EndTime > b.StartTime);

    if (hasConflict)
        return Results.BadRequest("Дорожка занята в выбранное время");

    var booking = new Booking
    {
        BookingId = Guid.NewGuid(),
        UserId = Guid.Parse(userId),
        BowlingLaneId = request.BowlingLaneId,
        StartTime = request.StartTime,
        EndTime = request.EndTime,
        Status = "Pending",
        CreatedAt = DateTime.UtcNow
    };

    db.Bookings.Add(booking);
    await db.SaveChangesAsync();

    return Results.Ok(booking);
})
.RequireAuthorization();

app.Run("http://localhost:5280");
