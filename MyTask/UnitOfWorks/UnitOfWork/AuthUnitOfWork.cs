using System.Data.SqlTypes;

namespace MyTask;

public class AuthUnitOfWork : BaseUnitOfWorkSetting<User>, IAuthUnitOfWork
{
    private readonly IUserRepository _repository;
    private readonly IJwtProvider _jwtProvider;
    private readonly RefreshTokenValidator _refreshTokenValidator;
    private readonly JwtRefreshOptions _jwtRefreshOptions;
    private readonly JwtAccessOptions _jwtAccessOptions;
    private readonly IUserUnitOfWork _userUnitOfWork;

    public AuthUnitOfWork(IUserRepository repository, IJwtProvider jwtProvider
        , RefreshTokenValidator refreshTokenValidator,IOptions<JwtRefreshOptions> jwtRefreshOptions,
        IOptions<JwtAccessOptions> jwtAccessOptions, IUserUnitOfWork userUnitOfWork) : base(repository)
    {
        _repository = repository;
        _jwtProvider = jwtProvider;
        _refreshTokenValidator = refreshTokenValidator;
        _jwtRefreshOptions = jwtRefreshOptions.Value;
        _jwtAccessOptions = jwtAccessOptions.Value;
        _userUnitOfWork = userUnitOfWork;
    }
    public async Task<Token> Register(UserRequest userRequest , string rootPath)
    {
        User user = await _userUnitOfWork.MapFromUserRequestToUser(userRequest , rootPath);
        user.RefreshToken = CreateNewRefreshToken();

        await this.Create(user);

        Token token = new()
        {
            AccessToken = _jwtProvider.GenrateAccessToken(user),
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtAccessOptions.ExpireTimeInMintes),
            RefreshToken = user.RefreshToken.Value,
            RefreshTokenExpiresAtExpires = user.RefreshToken.ExpireAt,
            Role = user.Role
        };

        return token;
    }

    public async Task<Token> Login(LoginRequest request)
    {
        User userFromDb = await _repository.GetByMail(request.Email);


        if (!BCrypt.Net.BCrypt.Verify(request.password, userFromDb.Password))
            throw new ArgumentException("wrong password");

        if (userFromDb.RefreshToken == null)
        {
            userFromDb.RefreshToken = CreateNewRefreshToken();
            await Update(userFromDb);
        }

        if (!_refreshTokenValidator.Validate(userFromDb.RefreshToken.Value))
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

        userFromDb.RefreshToken = null;
        await Update(userFromDb);
    }

    public async Task<Token> Refresh(string refreshToken)
    {
        User userFromDb = await _repository.GetByToken(refreshToken);

        Token token = new()
        {
            AccessToken = _jwtProvider.GenrateAccessToken(userFromDb),
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtAccessOptions.ExpireTimeInMintes),
            RefreshToken = refreshToken,
            RefreshTokenExpiresAtExpires = userFromDb.RefreshToken.ExpireAt,
            Role = userFromDb.Role
        };

        return token;
    }

    public async Task<Token> UpdatePassword(PasswordRequest password, Guid id)
    {
        User userFromDb = await _repository.Get(id);

        if (!BCrypt.Net.BCrypt.Verify(password.Password, userFromDb.Password))
            throw new ArgumentException("wrong password");

        userFromDb.Password = BCrypt.Net.BCrypt.HashPassword(password.NewPassword);

        userFromDb.RefreshToken = CreateNewRefreshToken();

        await Update(userFromDb);

        Token newToken = new()
        {
            AccessToken = _jwtProvider.GenrateAccessToken(userFromDb),
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtAccessOptions.ExpireTimeInMintes),
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
}
