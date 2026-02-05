using BronyBowling.Shared.Auth;
using BronyBowling.Shared.Data;
using BronyBowling.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new()
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

app.UseHttpsRedirection();
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
    Booking booking,
    ClaimsPrincipal user,
    ApplicationDbContext db) =>
{
    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
    if (userId is null)
        return Results.Unauthorized();

    var hasConflict = await db.Bookings.AnyAsync(b =>
        b.LaneId == booking.LaneId &&
        b.Status != "Cancelled" &&
        booking.StartTime < b.EndTime &&
        booking.EndTime > b.StartTime);

    if (hasConflict)
        return Results.BadRequest("Дорожка занята в выбранное время");

    booking.BookingId = Guid.NewGuid();
    booking.UserId = Guid.Parse(userId);
    booking.CreatedAt = DateTime.UtcNow;
    booking.Status = "Pending";

    db.Bookings.Add(booking);
    await db.SaveChangesAsync();

    return Results.Ok(booking);
})
.RequireAuthorization();

app.Run("http://localhost:5280");
