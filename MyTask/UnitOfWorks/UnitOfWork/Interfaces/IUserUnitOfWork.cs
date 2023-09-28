namespace MyTask;

public interface IUserUnitOfWork : IBaseUnitOfWorkSetting<User>
{
    Task<User>MapFromUserRequestToUser(UserRequest request, string rootPath, User? user = null);
}
