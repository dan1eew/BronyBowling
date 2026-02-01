using Microsoft.IdentityModel.Tokens;
using System.Text;

public class AuthOptions
{
    public const string ISSUER = "BronyBowlingAuthServer";
    public const string AUDIENCE = "BronyBowlingClient";

    private const string KEY = "SUPER_SECRET_KEY_123456789_BRONY_BOWLING";

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
        => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}
