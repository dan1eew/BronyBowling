namespace BronyBowling.Shared.Models;

public class Booking
{
    public Guid BookingId { get; set; }

    public Guid UserId { get; set; }

    public int BowlingLaneId { get; set; }      // FK
    public BowlingLane Lane { get; set; }       // navigation

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
}
