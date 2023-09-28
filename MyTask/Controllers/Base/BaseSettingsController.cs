namespace MyTask.Controllers;

public class BaseSettingsController<TEntity> : BaseController<TEntity>
    where TEntity : BaseEntitySetting
{
    private readonly IBaseUnitOfWorkSetting<TEntity> _unitOfWork;
    public BaseSettingsController(IBaseUnitOfWorkSetting<TEntity> unitOfWork) : base(unitOfWork) 
        => _unitOfWork = unitOfWork;

    protected virtual async Task<IActionResult> Search([FromRoute] string searchText)
    { 
        IEnumerable<TEntity> entities = await _unitOfWork.Search(searchText);

        return Ok(entities);
    }
}