using MyTask;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection"))
                      .EnableDetailedErrors()
                      .EnableSensitiveDataLogging()
                      .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddDependencyInjectionService();

builder.Services.AddHttpContextAccessor();

builder.Services.AddOptionService();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

var app = builder.Build();


app.UseMiddleware<TransactionMiddleware>();



app.UseMiddleware<GlobalErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
