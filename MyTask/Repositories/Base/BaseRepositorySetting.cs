namespace MyTask;

public class BaseRepositorySetting<TEntity> : BaseRepository<TEntity>,
    IBaseRepositorySetting<TEntity> where TEntity : BaseEntitySetting
{
    public BaseRepositorySetting(ApplicationDbContext context) : base(context) { }

    public virtual async Task<IEnumerable<TEntity>> Search(string searchText)
        => await Task.Run(() => dbSet.Where(e => e.Name.Contains(searchText)));
}
