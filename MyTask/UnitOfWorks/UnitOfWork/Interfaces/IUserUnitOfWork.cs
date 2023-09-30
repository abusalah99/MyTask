namespace MyTask;
public interface IUserUnitOfWork : IBaseUnitOfWorkSetting<User>
{
    Task<User>MapFromUserRequestToUser(UserRequest request);

    Task<byte[]> GetUserImage();
    Task ChangeUserRoleToCreator(Guid userId);

    Task ChangeUserRoleToAdmin(Guid userId);
}
