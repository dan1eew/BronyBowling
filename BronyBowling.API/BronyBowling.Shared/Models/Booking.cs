namespace BronyBowling.Shared.Models;

public class Booking
{
    public Guid BookingId { get; set; }
    public Guid? UserId { get; set; }  
    public string? GuestFullName { get; set; }
    public string? GuestPhone { get; set; }
    public int BowlingLaneId { get; set; }
    public BowlingLane Lane { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
}
