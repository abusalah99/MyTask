namespace MyTask;

public interface IAuthUnitOfWork : IBaseUnitOfWorkSetting<User>
{
    Task<Token> Login(LoginRequest request);
    Task<Token> Register(UserRequest userRequest);
    Task<Token> Refresh(string refreshToken);
    Task Logout(string refreshToken);
    Task<Token> UpdatePassword(PasswordRequest password);
}
