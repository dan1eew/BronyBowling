namespace BronyBowling.Shared.Validation;

public class PhoneValidator
{
    public static bool IsValid(string? phone)
    {
        if (string.IsNullOrEmpty(phone)) return false;

        return phone.Length == 11 && phone.All(char.IsDigit);
    }
    public static string? Validate(string? phone)
    {
        if (string.IsNullOrEmpty(phone)) return "Телефон обязателен";

        if (phone.Length != 11) return "Телефон должен содержать 11 цифр";

        if (!phone.All(char.IsDigit)) return "Телефон должен содержать только цифры";

        return null;
    }
}
