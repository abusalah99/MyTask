namespace MyTask;

public class AuthUnitOfWork : BaseUnitOfWorkSetting<User>, IAuthUnitOfWork
{
    private readonly IUserRepository _repository;
    private readonly IJwtProvider _jwtProvider;
    private readonly RefreshTokenValidator _refreshTokenValidator;
    private readonly JwtRefreshOptions _jwtRefreshOptions;
    private readonly JwtAccessOptions _jwtAccessOptions;
    private readonly IUserUnitOfWork _userUnitOfWork;
    private readonly IHttpContextAccessor _httpContext;

    public AuthUnitOfWork(IUserRepository repository, IJwtProvider jwtProvider
        , RefreshTokenValidator refreshTokenValidator,IOptions<JwtRefreshOptions> jwtRefreshOptions,
        IOptions<JwtAccessOptions> jwtAccessOptions, IUserUnitOfWork userUnitOfWork, IHttpContextAccessor httpContext) : base(repository)
    {
        _repository = repository;
        _jwtProvider = jwtProvider;
        _refreshTokenValidator = refreshTokenValidator;
        _jwtRefreshOptions = jwtRefreshOptions.Value;
        _jwtAccessOptions = jwtAccessOptions.Value;
        _userUnitOfWork = userUnitOfWork;
        _httpContext = httpContext;
    }

    public async Task<Token> Register(UserRequest userRequest)
    {
        User user = await _userUnitOfWork.MapFromUserRequestToUser(userRequest);

        user.RefreshToken = CreateNewRefreshToken();

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        await _userUnitOfWork.Create(user);

        Token token = new()
        {
            AccessToken = _jwtProvider.GenrateAccessToken(user),
            AccessTokenExpiresAt = DateTime.Now.AddMinutes(_jwtAccessOptions.ExpireTimeInMintes),
            RefreshToken = user.RefreshToken.Value,
            RefreshTokenExpiresAtExpires = user.RefreshToken.ExpireAt,
            Role = user.Role
        };

        return token;
    }

    public async Task<Token> Login(LoginRequest request)
    {
        User userFromDb = await _repository.GetByMail(request.Email);


        if (PasswordNotEqual(request.password, userFromDb.Password))
            throw new ArgumentException("wrong password");

        if (TokenNotExist(userFromDb.RefreshToken) || TokenNotValid(userFromDb.RefreshToken))
        {
            userFromDb.RefreshToken = CreateNewRefreshToken();
            await Update(userFromDb);
        }

        Token token = new()
        {
            AccessToken = _jwtProvider.GenrateAccessToken(userFromDb),
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtAccessOptions.ExpireTimeInMintes),
            RefreshToken = userFromDb.RefreshToken.Value,
            RefreshTokenExpiresAtExpires = userFromDb.RefreshToken.ExpireAt,
            Role = userFromDb.Role
        };

        return token;
    }

    public async Task Logout(string refreshToken)
    {
        User userFromDb = await _repository.GetByToken(refreshToken);

        userFromDb.RefreshToken = new();

        await Update(userFromDb);
    }

    public async Task<Token> Refresh(string refreshToken)
    {
        User userFromDb = await _repository.GetByToken(refreshToken);

        Token token = new()
        {
            AccessToken = _jwtProvider.GenrateAccessToken(userFromDb),
            AccessTokenExpiresAt = DateTime.Now.AddMinutes(_jwtAccessOptions.ExpireTimeInMintes),
            RefreshToken = userFromDb.RefreshToken.Value,
            RefreshTokenExpiresAtExpires = userFromDb.RefreshToken.ExpireAt,
            Role = userFromDb.Role
        };

        return token;
    }

    public async Task<Token> UpdatePassword(PasswordRequest password)
    {
        var context = _httpContext?.HttpContext;

        Guid id = GetUserIdFromClaims(context);

        User userFromDb = await _repository.Get(id);

        if (PasswordNotEqual(password.NewPassword , userFromDb.Password))
            throw new ArgumentException("wrong password");

        userFromDb.Password = BCrypt.Net.BCrypt.HashPassword(password.NewPassword);

        userFromDb.RefreshToken = CreateNewRefreshToken();

        await Update(userFromDb);

        Token newToken = new()
        {
            AccessToken = _jwtProvider.GenrateAccessToken(userFromDb),
            AccessTokenExpiresAt = DateTime.Now.AddMinutes(_jwtAccessOptions.ExpireTimeInMintes),
            RefreshToken = userFromDb.RefreshToken.Value,
            RefreshTokenExpiresAtExpires = userFromDb.RefreshToken.ExpireAt,
            Role = userFromDb.Role
        };

        return newToken;
    }

    private RefreshToken CreateNewRefreshToken()
    {
        string refreshToken = _jwtProvider.GenrateRefreshToken();

        RefreshToken newRefreshToken = new()
        {
            Value = refreshToken,
            CreatedAt = DateTime.Now,
            ExpireAt = DateTime.Now.AddMonths(_jwtRefreshOptions.ExpireTimeInMonths)
        };

        return newRefreshToken;
    }
    private bool TokenNotValid(RefreshToken token) => !_refreshTokenValidator.Validate(token.Value);

    private bool TokenNotExist(RefreshToken token) => token.Value.IsNullOrEmpty();
    private bool PasswordNotEqual(string password, string rightPassword)
        => !BCrypt.Net.BCrypt.Verify(password, rightPassword);
}
