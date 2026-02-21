namespace BronyBowling.Shared.Models;

public class Tariff
{
    public int TariffId { get; set; }

    public int CenterId { get; set; }
    public BowlingCenter Center { get; set; } = null!;

    public decimal WeekdayPrice { get; set; }
    public decimal WeekendPrice { get; set; }

    public bool IsActive { get; set; }
}
