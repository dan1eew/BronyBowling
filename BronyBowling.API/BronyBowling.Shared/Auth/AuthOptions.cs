using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BronyBowling.Shared.Auth;
public class AuthOptions
{
    public const string ISSUER = "BronyBowlingAuthServer";
    public const string AUDIENCE = "BronyBowlingClient";
    private const string KEY = "DANTE_HELSINKI_17092005_BRONY_BOWLING";

    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
           new(Encoding.UTF8.GetBytes(KEY));
}