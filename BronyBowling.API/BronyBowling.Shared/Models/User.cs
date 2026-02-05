namespace BronyBowling.Shared.Models;

/// <summary> Пользователь системы </summary>
public class User
{
    public Guid UserId { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? City { get; set; }
    public DateTime CreatedAt { get; set; }
}
