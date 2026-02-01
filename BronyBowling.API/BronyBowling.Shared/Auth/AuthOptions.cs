using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BronyBowling.Shared.Auth;

/// <summary> Общие настройки JWT для всех сервисов </summary>
public static class AuthOptions
{
    public const string ISSUER = "BronyBowlingAuthServer"; // Кто выдал токен
    public const string AUDIENCE = "BronyBowlingClient"; // Для кого предназначен токен
    private const string KEY = "DANTE_HELSINKI_17092005_BRONY_BOWLING"; // Ключ для создания JWT

    /// <summary> Ключ для подписи JWT </summary>
    public static SymmetricSecurityKey SecurityKey =>
           new(Encoding.UTF8.GetBytes(KEY));
}