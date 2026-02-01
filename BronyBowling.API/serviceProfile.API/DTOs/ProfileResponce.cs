namespace serviceProfile.API.DTOs;
public class ProfileResponse
{
    public string PhoneNumber { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public DateTime? BirthDate { get; set; }
    public string? City { get; set; }
}
