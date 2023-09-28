namespace MyTask;

public interface IAuthUnitOfWork : IBaseUnitOfWorkSetting<User>
{
    Task<Token> Login(LoginRequest request);
    Task<Token> Register(UserRequest userRequest, string rootPath);
    Task<Token> Refresh(string refreshToken);
    Task Logout(string refreshToken);
}
