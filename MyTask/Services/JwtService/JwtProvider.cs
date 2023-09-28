namespace MyTask;

public class JwtProvider : IJwtProvider
{
    private readonly JwtAccessOptions _jwtAccessOptions;
    private readonly JwtRefreshOptions _jwtRefreshOptions;

    public JwtProvider(IOptions<JwtAccessOptions> jwtAccessOptions,
        IOptions<JwtRefreshOptions> jwtRefreshOptions)
    {
        _jwtAccessOptions = jwtAccessOptions.Value;
        _jwtRefreshOptions = jwtRefreshOptions.Value;
    }

    public string GenrateAccessToken(User user)
    {
        var claims = new List<Claim>()
        {
            new("Id", user.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, $"{user.Name}"),
            new(ClaimTypes.Role, user.Role),
        };
 
       string token = TokenGenrator(
            _jwtAccessOptions.SecretKey,
            claims,
            null,
            null,
            DateTime.UtcNow.AddMinutes(_jwtAccessOptions.ExpireTimeInMintes));

        return token;
    }

    public string GenrateRefreshToken()
    {

        string token = TokenGenrator(
            _jwtRefreshOptions.SecretKey,
            null,
            null,
            null,
            DateTime.UtcNow.AddMonths(_jwtRefreshOptions.ExpireTimeInMonths));

        return token;
    }

    private string TokenGenrator(
        string secretKey,
        List<Claim>? claims,
        String? issuser,
        String? audience,
        DateTime expireDate)
    {
        var SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
           issuser,
           audience,
           claims,
           null,
           expireDate,
           SigningCredentials);

        string tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return tokenValue;
    }

}
