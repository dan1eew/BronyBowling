namespace serviceLogin.API.DTOs;

/// <summary> DTO для регистрации </summary>
public class RegisterRequest
{
    public string PhoneNumber { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; } = null!;
    public DateTime? BirthDate { get; set; }
    public string? City { get; set; }
}