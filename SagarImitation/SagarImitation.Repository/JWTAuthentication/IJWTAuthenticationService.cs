using SagarImitation.Model.Token;
using SagarImitation.Model.Token;

namespace SagarImitation.Service.JWTAuthentication
{
    public interface IJWTAuthenticationService
    {
        AccessTokenModel GenerateToken(TokenModel userToken, string JWT_Secret, int JWT_Validity_Mins);
        TokenModel GetUserTokenData(string? jwtToken = null);
    }
}