namespace BronyBowling.Shared.Models;

public class BowlingLane
{
    public int BowlingLaneId { get; set; }   // PK
    public int Number { get; set; }
    public bool IsActive { get; set; }
}