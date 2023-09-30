using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyTask.Controllers;

public class BaseController<TEntity> : ControllerBase
    where TEntity : BaseEntity
{
    private readonly IBaseUnitOfWork<TEntity> _unitOfWork;
    public BaseController(IBaseUnitOfWork<TEntity> unitOfWork) => _unitOfWork = unitOfWork;

    protected virtual async Task<IActionResult> Create(TEntity entity)
    {
        await _unitOfWork.Create(entity);
        
        return Ok(new { Reponse = $"{typeof(TEntity).Name} Created" });
    }

    protected virtual async Task<IActionResult> Read() => Ok(new { Reponse = await _unitOfWork.Read() });
    protected virtual async Task<IActionResult> Read(Guid id) => Ok(new { Reponse = await _unitOfWork.Read(id) });

    protected async virtual Task<IActionResult> Update(TEntity entity)
    {
        await _unitOfWork.Update(entity);

        return Ok (new { Reponse = $"{typeof(TEntity).Name} Updated" });
    }

    protected async virtual Task<IActionResult> Remove(Guid id) 
    {
        await _unitOfWork.Delete(id);

        return Ok(new { Reponse = $"{typeof(TEntity).Name} Deleted" });
    }

    protected void SetCookie(string name, string value, DateTime expireTime)
    => Response.Cookies.Append(name, value
        , new CookieOptions()
        {
            SameSite = SameSiteMode.None,
            HttpOnly = true,
            Expires = expireTime,
            Secure = true,
        });

    protected Guid GetUserId()
    {
        var id = User.FindFirst("Id") ?? new("Id", Guid.Empty.ToString());

        return new(id.Value);
    }
}
