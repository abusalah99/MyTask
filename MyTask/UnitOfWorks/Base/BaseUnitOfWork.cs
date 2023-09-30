namespace MyTask;

public class BaseUnitOfWork<TEntity> : IBaseUnitOfWork<TEntity> where TEntity : BaseEntity
{
    private readonly IBaseRepository<TEntity> _repository;

    public BaseUnitOfWork(IBaseRepository<TEntity> repository) => _repository = repository;

    public virtual async Task<IEnumerable<TEntity>> Read() => await _repository.Get();
    public virtual async Task<TEntity> Read(Guid id) => await _repository.Get(id);

    public virtual async Task Create(TEntity entity) => await _repository.Add(entity);

    public virtual async Task Delete(Guid id) => await _repository.Remove(id);

    public virtual async Task Update(TEntity entity) => await _repository.Update(entity);

    protected Guid GetUserIdFromClaims(HttpContext? context)
    {

        if (context == null)
            throw new InvalidOperationException("This operation requires an active HTTP context.");

        var claimsId = context.User.FindFirst("Id") ?? new("Id", Guid.Empty.ToString());

        return new(claimsId.Value);
    }
}
