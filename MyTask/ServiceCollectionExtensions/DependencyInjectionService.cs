namespace MyTask;

public static class DependencyInjectionService
{
    public static void AddDependencyInjectionService(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddSingleton(typeof(IBaseRepositorySetting<>), typeof(BaseRepositorySetting<>));
        services.AddSingleton(typeof(IBaseUnitOfWork<>), typeof(BaseUnitOfWork<>));
        services.AddSingleton(typeof(IBaseUnitOfWorkSetting<>), typeof(BaseUnitOfWorkSetting<>));

        services.AddSingleton<IImageConverter, ImageConverter>();
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddSingleton<RefreshTokenValidator>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserUnitOfWork,  UserUnitOfWork>();

        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IAssignmentUnitOfWork, AssignmentUnitOfWork>();

        services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();

        services.AddTransient<TransactionMiddleware>();
        services.AddTransient<GlobalErrorHandlerMiddleware>();
    }
}
