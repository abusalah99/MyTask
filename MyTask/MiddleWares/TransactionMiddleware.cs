namespace MyTask;

public class TransactionMiddleware : IMiddleware
{
    private readonly ApplicationDbContext _dbContext;

    public TransactionMiddleware(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Method != HttpMethods.Get || context.Request.Method != HttpMethods.Options)
        {
                IDbContextTransaction? transaction = null;
            try
            {
                transaction = await _dbContext.Database.BeginTransactionAsync();

                await next(context);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                transaction?.Rollback();
                throw;
            }
        }
    }
}
