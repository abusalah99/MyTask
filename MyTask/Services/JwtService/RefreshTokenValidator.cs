namespace MyTask;

public class RefreshTokenValidator
{
    private readonly JwtRefreshOptions _jwtRefreshOptions;

    public RefreshTokenValidator(IOptions<JwtRefreshOptions> jwtRefreshOptions)
        => _jwtRefreshOptions = jwtRefreshOptions.Value;

    public bool Validate(string refreshToken)
    {
        TokenValidationParameters validationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtRefreshOptions.SecretKey)),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
        };
        
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(refreshToken, validationParameters,
                out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

}