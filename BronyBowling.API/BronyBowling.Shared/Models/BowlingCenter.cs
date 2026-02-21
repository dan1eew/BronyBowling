namespace BronyBowling.Shared.Models;

public class BowlingCenter
{
    public int CenterId { get; set; }

    public string Name { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string House { get; set; } = null!;

    public TimeOnly WeekdayOpen { get; set; }
    public TimeOnly WeekdayClose { get; set; }

    public TimeOnly WeekendOpen { get; set; }
    public TimeOnly WeekendClose { get; set; }

    public int LanesCount { get; set; }
    public bool IsActive { get; set; }

    public Tariff? Tariff { get; set; } = null!;
}
