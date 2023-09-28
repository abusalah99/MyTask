namespace MyTask;

public class BaseUnitOfWorkSetting<TEntity>
    : BaseUnitOfWork<TEntity>, IBaseUnitOfWorkSetting<TEntity> where TEntity : BaseEntitySetting
{
    private readonly IBaseRepositorySetting<TEntity> _repository;

    public BaseUnitOfWorkSetting(IBaseRepositorySetting<TEntity> repository)
        : base(repository) => _repository = repository;

    public virtual async Task<IEnumerable<TEntity>> Search(string searchText) 
        => await _repository.Search(searchText);
}
