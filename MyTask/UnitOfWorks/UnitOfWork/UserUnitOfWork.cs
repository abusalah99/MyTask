namespace MyTask;
public partial class UserUnitOfWork : BaseUnitOfWorkSetting<User>, IUserUnitOfWork
{
    private readonly IFileService _fileService;
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IUserRepository _repository;

    public UserUnitOfWork(IUserRepository repository, IWebHostEnvironment env,
        IFileService fileService, IHttpContextAccessor httpContext) : base(repository)
    {
        _fileService = fileService;
        _env = env;
        _httpContext = httpContext;
        _repository = repository;
    }

    public override async Task Create(User user)
    {
        user.Role = "User";
        await base.Create(user);
    }

    public override async Task Update(User user)
    {
        var context = _httpContext?.HttpContext;

        Guid id = GetUserIdFromClaims(context);

        User userFromDb = await _repository.Get(id);

        user.Password = userFromDb.Password;
        user.Id = userFromDb.Id;
        user.RefreshToken = userFromDb.RefreshToken;
        user.Role = userFromDb.Role;

        if (user.ProfilePhotoUrl.IsNullOrEmpty())
            user.ProfilePhotoUrl = userFromDb.ProfilePhotoUrl;

        await base.Update(user);
    }

    public async Task<User> MapFromUserRequestToUser(UserRequest request)
    {
        var context = _httpContext?.HttpContext?.Request;

        if (context == null)
            throw new InvalidOperationException("This operation requires an active HTTP context.");

        var host = context.Host.Host;
        var port = context.Host.Port;

        var url = $"{context.Scheme}://{host}:{port}";

        User user = new();

        if (request.Id == Guid.Empty)
            user.Id = Guid.NewGuid();
        else
            user.Id = request.Id;

        user.Name = request.Name;
        user.Password = request.Password;
        user.Email = request.Email;
        user.Phone = request.Phone;

        if (request.ProfileImage != null)
        {
            await _fileService.DeleteFileIfExists($"{_env.ContentRootPath}/Assets/Images/ProfilePhotos/{user.Id}.png");
            await _fileService.Save(request.ProfileImage, $"{_env.ContentRootPath}/Assets/Images/ProfilePhotos/{user.Id}.png");
            user.ProfilePhotoUrl = $"{url}/api/user/ProfilePhoto";
        }

        return user;
    }

    public async override Task Delete(Guid id)
    {
        await _fileService.DeleteFileIfExists($"{_env.ContentRootPath}/Assets/Images/ProfilePhotos/{id}.png");

        await base.Delete(id);
    }

    public async Task<byte[]> GetUserImage()
    {
        var context = _httpContext?.HttpContext;

        Guid id = GetUserIdFromClaims(context);

        try
        {
            return await _fileService.Get($"{_env.ContentRootPath}/Assets/Images/ProfilePhotos/{id}.png");
        }
        catch
        {
            return await _fileService.Get($"{_env.ContentRootPath}/Assets/Images/ProfilePhotos/DefualtUser.png");
        }
    }
    public async Task ChangeUserRoleToAdmin(Guid userId) => await ChangeUserRole(userId, "Admin");

    public async Task ChangeUserRoleToCreator(Guid userId) => await ChangeUserRole(userId, "Creator");


    private async Task ChangeUserRole(Guid userId, string role)
    {
        User userFromDb = await Read(userId);

        userFromDb.Role = role;
    }
}