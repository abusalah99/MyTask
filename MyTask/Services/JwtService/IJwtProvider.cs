namespace MyTask;

public interface IJwtProvider
{
    string GenrateAccessToken(User user);
    string GenrateRefreshToken();

}
