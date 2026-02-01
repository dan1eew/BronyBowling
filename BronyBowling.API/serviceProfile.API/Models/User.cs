namespace serviceProfile.API.Models;

/// <summary> Упрощённая модель пользователя для сервиса профиля </summary>
public class User
{
    public Guid UserId { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public DateTime? BirthDate { get; set; }
    public string? City { get; set; }
}
