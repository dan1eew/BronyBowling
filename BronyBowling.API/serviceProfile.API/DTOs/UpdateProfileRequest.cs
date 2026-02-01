namespace serviceProfile.API.DTOs;

public class UpdateProfileRequest
{
    public string FullName { get; set; } = null!;
    public DateTime? BirthDate { get; set; }
    public string? City { get; set; }
}