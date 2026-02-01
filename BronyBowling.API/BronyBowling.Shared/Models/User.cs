namespace serviceLogin.API.Models;
public class User
{
    public Guid UserId { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public DateTime? BirthDate { get; set; }
    public string? City { get; set; }
    public DateTime CreateAt { get; set; }
}
