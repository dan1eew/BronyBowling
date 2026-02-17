namespace serviceProfile.API.DTOs;

public class UpdateProfileRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? City { get; set; }
}