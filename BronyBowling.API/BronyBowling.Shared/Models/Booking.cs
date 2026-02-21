namespace BronyBowling.Shared.Models;

public class Booking
{
    public int BookingId { get; set; }

    public int CenterId { get; set; }
    public BowlingCenter Center { get; set; } = null!;

    public int LaneNumber { get; set; }

    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public string? GuestName { get; set; }
    public string? GuestPhone { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string BookingCode { get; set; } = null!;
    public string Status { get; set; } = "Created";

    public DateTime CreatedAt { get; set; }
}

