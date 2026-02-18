namespace BronyBowling.Shared.Models;
public class BowlingCenter
{
    public int BowlingCenterId { get; set; }
    public string Name { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string House { get; set; } = null!;
    public string WorkingHours { get; set; } = null!; 
    public bool IsActive { get; set; }
    public List<BowlingLane> Lanes { get; set; } = new();
}
