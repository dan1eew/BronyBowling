using System.Security.Cryptography;
using System.Text;

namespace serviceLogin.API.Services;

/// <summary> Хеширование паролей </summary>
public sealed class PasswordHasher
{
    public string Hash(string password)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hash);
    }

    public bool Verify(string password, string hash)
        => Hash(password) == hash;
}
