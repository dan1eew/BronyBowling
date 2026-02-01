namespace serviceLogin.API.DTOs;
public class RegisterRequest
{
    public string PhoneNumber { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FullName { get; set; } = null!;

    // необязательные
    public DateTime? BirthDate { get; set; }
    public string? City { get; set; }
}
