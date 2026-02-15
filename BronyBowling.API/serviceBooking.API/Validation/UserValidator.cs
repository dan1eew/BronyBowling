namespace serviceBooking.API.Validation;
public class UserValidator
{
    public static List<string> ValidateRegistration(
        string? phone,
        string? password,
        string? firstName,
        string? lastName)
    {
        var errors = new List<string>();

        var phoneError = PhoneValidator.Validate(phone);
        if (phoneError != null)
            errors.Add(phoneError);

        if (string.IsNullOrWhiteSpace(password))
            errors.Add("Пароль обязателен");

        else if (password.Length < 6)
            errors.Add("Пароль минимум 6 символов");

        if (string.IsNullOrWhiteSpace(firstName))
            errors.Add("Имя обязательно");

        if (string.IsNullOrWhiteSpace(lastName))
            errors.Add("Фамилия обязательна");

        return errors;
    }
}
