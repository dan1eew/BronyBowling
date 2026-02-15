namespace serviceBooking.API.Validation;
public class ValidationResult
{
    public bool Success => Errors.Count == 0;
    public List<string> Errors { get; set; } = new();
}
