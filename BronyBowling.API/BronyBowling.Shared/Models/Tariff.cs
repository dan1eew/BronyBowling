namespace BronyBowling.Shared.Models;
public class Tariff
{
    public int TariffId { get; set; }
    public int BowlingLaneId { get; set; }
    public BowlingLane Lane { get; set; } = null!;
    public int DayOfWeek { get; set; } 
    public decimal PricePerHour { get; set; }
    public bool IsActive { get; set; }
}
