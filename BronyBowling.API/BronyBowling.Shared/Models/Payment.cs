namespace BronyBowling.Shared.Models;

public class Payment
{
    public int PaymentId { get; set; }

    public string PhoneNumber { get; set; } = null!;
    public decimal Amount { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string CenterName { get; set; } = null!;
    public int LaneNumber { get; set; }

    public DateTime CreatedAt { get; set; }
}
