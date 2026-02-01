namespace serviceLogin.API.Models;

/// <summary> Пользователь системы </summary>
public class User
{
    public Guid UserId { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public DateTime? BirthDate { get; set; }
    public string? City { get; set; }
    public DateTime CreatedAt { get; set; }
}
