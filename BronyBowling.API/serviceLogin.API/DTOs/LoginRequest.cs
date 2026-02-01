namespace serviceLogin.API.DTOs;
public class LoginRequest
{
    public string PhoneNumber { get; set; } = null!;
    public string Password { get; set; } = null!;
}
