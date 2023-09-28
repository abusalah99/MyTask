namespace MyTask;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task Add(TEntity entity);

    Task<IEnumerable<TEntity>> Get();

    Task<TEntity> Get(Guid id);

    Task Update(TEntity entity);

    Task Remove(Guid id);
    Task Remove(TEntity entity);
}
