namespace BronyBowling.Shared.Models;

public class Admin
{
    public Guid AdminId { get; set; }
    public string Login { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}