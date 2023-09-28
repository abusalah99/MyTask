namespace MyTask;

public interface IBaseRepositorySetting<TEntity> 
    : IBaseRepository<TEntity> where TEntity : BaseEntitySetting
{
    Task<IEnumerable<TEntity>> Search(string searchText);
}
