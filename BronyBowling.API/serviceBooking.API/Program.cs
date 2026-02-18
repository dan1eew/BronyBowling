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
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
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

static bool TryParseWorkingHours(string workingHours, out TimeOnly open, out TimeOnly close)
{
    open = default;
    close = default;

    if (string.IsNullOrWhiteSpace(workingHours))
        return false;

    var parts = workingHours.Split('-', StringSplitOptions.TrimEntries);

    if (parts.Length != 2)
        return false;

    return TimeOnly.TryParse(parts[0], out open)
        && TimeOnly.TryParse(parts[1], out close);
}

static bool IsWithinWorkingHours(DateTime startUtc, DateTime endUtc, string workingHours)
{
    if (!TryParseWorkingHours(workingHours, out var open, out var close))
        return true;

    var startLocal = TimeOnly.FromDateTime(startUtc.ToLocalTime());
    var endLocal = TimeOnly.FromDateTime(endUtc.ToLocalTime());

    if (open <= close)
        return startLocal >= open && endLocal <= close;

    return (startLocal >= open || startLocal <= close)
        && (endLocal >= open || endLocal <= close);
}

// -------------------- ENDPOINTS --------------------

app.MapGet("/lanes", async (ApplicationDbContext db) =>
{
    var lanes = await db.BowlingLanes
        .Where(l => l.IsActive)
        .OrderBy(l => l.Number)
        .Select(l => new
        {
            laneId = l.LaneId,
            number = l.Number,
            centerId = l.BowlingCenterId
        })
        .ToListAsync();

    return Results.Ok(lanes);
});

app.MapGet("/lanes/available", async (
    DateTime start,
    DateTime end,
    ApplicationDbContext db) =>
{
    start = EnsureUtc(start);
    end = EnsureUtc(end);

    if (end <= start)
        return Results.BadRequest("Некорректный интервал");

    var requestedRange = new NpgsqlRange<DateTime>(start, true, end, false);

    // Занятые дорожки одним запросом
    var busyLaneIds = await db.Bookings
        .Where(b => b.Status == "Created" || b.Status == "Confirmed" &&
                    b.TimeRange.Overlaps(requestedRange))
        .Select(b => b.BowlingLaneId)
        .Distinct()
        .ToListAsync();

    var lanes = await db.BowlingLanes
        .Include(l => l.Center)
        .Where(l => l.IsActive &&
                    l.Center.IsActive &&
                    !busyLaneIds.Contains(l.LaneId))
        .ToListAsync();

    var available = lanes
        .Where(l => IsWithinWorkingHours(start, end, l.Center.WorkingHours))
        .OrderBy(l => l.Number)
        .Select(l => new
        {
            laneId = l.LaneId,
            number = l.Number,
            center = l.Center.Name
        });

    return Results.Ok(available);
});

app.MapPost("/bookings", async (
    CreateBookingRequest request,
    ClaimsPrincipal user,
    ApplicationDbContext db) =>
{
    var startUtc = EnsureUtc(request.StartTime);
    var endUtc = EnsureUtc(request.EndTime);

    var errors = BookingValidator.Validate(startUtc, endUtc, request.BowlingLaneId);
    if (errors.Any())
        return Results.BadRequest(errors);

    var lane = await db.BowlingLanes
        .Include(l => l.Center)
        .FirstOrDefaultAsync(l => l.LaneId == request.BowlingLaneId);

    if (lane is null || !lane.IsActive || !lane.Center.IsActive)
        return Results.BadRequest("Дорожка недоступна");

    if (!IsWithinWorkingHours(startUtc, endUtc, lane.Center.WorkingHours))
        return Results.BadRequest("Вне времени работы центра");

    var newRange = new NpgsqlRange<DateTime>(startUtc, true, endUtc, false);

    var hasConflict = await db.Bookings
        .Where(b => b.BowlingLaneId == request.BowlingLaneId &&
                    b.Status != "Cancelled")
        .AnyAsync(b => b.TimeRange.Overlaps(newRange));

    if (hasConflict)
        return Results.BadRequest("Дорожка занята в указанное время");

    var booking = new Booking
    {
        BowlingLaneId = request.BowlingLaneId,
        TimeRange = newRange,
        Status = "Created",
        CreatedAt = DateTime.UtcNow
    };

    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

    if (userId != null)
        booking.UserId = Guid.Parse(userId);
    else
    {
        if (string.IsNullOrWhiteSpace(request.GuestFullName) ||
            string.IsNullOrWhiteSpace(request.GuestPhone))
        {
            return Results.BadRequest("Введите ФИО и телефон");
        }

        booking.GuestFullName = request.GuestFullName;
        booking.GuestPhone = request.GuestPhone;
    }

    db.Bookings.Add(booking);
    await db.SaveChangesAsync();

    return Results.Ok(booking);
})
.AllowAnonymous();

app.Run("http://localhost:5280");