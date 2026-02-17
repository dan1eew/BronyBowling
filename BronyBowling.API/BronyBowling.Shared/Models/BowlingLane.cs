namespace BronyBowling.Shared.Models;

public class BowlingLane
{
    public int LaneId { get; set; }
    public int BowlingCenterId { get; set; }
    public BowlingCenter Center { get; set; } = null!;
    public int Number { get; set; }
    public bool IsActive { get; set; }
    public List<Tariff> Tariffs { get; set; } = new();
}