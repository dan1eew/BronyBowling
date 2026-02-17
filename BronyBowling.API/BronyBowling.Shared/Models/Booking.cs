using NpgsqlTypes;

namespace BronyBowling.Shared.Models;

public class Booking
{
    public Guid BookingId { get; set; }
    public Guid? UserId { get; set; }
    public string? GuestFullName { get; set; }
    public string? GuestPhone { get; set; }
    public Guid BowlingLaneId { get; set; }
    public BowlingLane Lane { get; set; } = null!;
    public NpgsqlRange<DateTime> TimeRange { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
}