namespace BronyBowling.Shared.Models;

/// <summary> Пользователь системы </summary>
public class User
{
    public Guid UserId { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
