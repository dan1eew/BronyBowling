namespace BronyBowling.Shared.Models;
public class Booking
{
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public int BowlingLanesId { get; set; }
    public BowlingLane Lane { get; set; }
    public DateTime StartTime {  get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
