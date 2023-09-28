namespace MyTask;

public class UserUnitOfWork : BaseUnitOfWorkSetting<User>, IUserUnitOfWork
{
    private readonly IFileSaver _fileSaver;

    public UserUnitOfWork(IUserRepository repository, IFileSaver fileSaver) : base(repository)
            => _fileSaver = fileSaver;
    
    public override async Task Create(User user)
    {
        user.Role = "User";
        await base.Create(user);
    }
    public override async Task Update(User user)
    {
        user.Role = "User";
        await base.Create(user);
    }

    public async Task<User> MapFromUserRequestToUser(UserRequest request, string rootPath, User? user = null)
    {
        User newUser = new();
        if (user != null)
            newUser = user;

        if (request.Id == Guid.Empty)
            newUser.Id = Guid.NewGuid();
        newUser.Name = request.Name;
        newUser.Email = request.Email;
        newUser.Password = request.Password;
        newUser.Phone = request.Phone;
        newUser.Role = request.Role;
        if (request.UserImage != null)
        {
            await _fileSaver.DeleteFileIfExists($"{rootPath}/ProfilePictures/{newUser.Id}.png");
            await _fileSaver.Save(request.UserImage, $"{rootPath}/ProfilePictures/{newUser.Id}.png");
            newUser.ProfilePhotoUrl = $"{rootPath}/ProfilePictures/{user.Id}.png";
        }

        return newUser;
    }
}