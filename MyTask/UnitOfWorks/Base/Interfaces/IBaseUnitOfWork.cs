namespace MyTask;

public interface IBaseUnitOfWork<TEntity> where TEntity : BaseEntity
{
    Task Create(TEntity entity);

    Task<IEnumerable<TEntity>> Read();
    Task<TEntity> Read(Guid id);

    Task Update(TEntity entity);

    Task Delete(Guid id);
    Task Delete(TEntity entity);
}
