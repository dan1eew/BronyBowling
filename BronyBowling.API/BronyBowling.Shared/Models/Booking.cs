using NpgsqlTypes;

namespace BronyBowling.Shared.Models;

public class Booking
{
    public int BookingId { get; set; }
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    public string? GuestFullName { get; set; }
    public string? GuestPhone { get; set; }
    public int BowlingLaneId { get; set; }
    public BowlingLane Lane { get; set; } = null!;
    public NpgsqlRange<DateTime> TimeRange { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
}