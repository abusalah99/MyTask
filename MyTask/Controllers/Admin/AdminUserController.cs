using Microsoft.AspNetCore.SignalR;

namespace MyTask.Controllers;

[Route("api/Admin/User")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminUserController : BaseControllerSetting<User>
{
    private readonly IUserUnitOfWork _unitOfWork;
    public AdminUserController(IUserUnitOfWork unitOfWork) : base(unitOfWork) => _unitOfWork = unitOfWork;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(new { Response = await Read() });
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id) => Ok(new { Response = await Read(id) });

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] UserRequest request)
    {
        User user = await _unitOfWork.MapFromUserRequestToUser(request);

        return await Create(user);
    }

    [HttpPut]
    public async Task<IActionResult> Put(UserRequest request)
    {
        User user = await _unitOfWork.MapFromUserRequestToUser(request);

        return await Update(user);
    }
    [HttpPut("{id}"), Route("MakeUserAsCreator")]
    public async Task<IActionResult> MakeUserAsCreator(Guid id)
    {
        await _unitOfWork.ChangeUserRoleToCreator(id);

        return Ok(new { Response = "UserRoleUpdated" });
    }
    [HttpPut("{id}"), Route("MakeUserAsAdmin")]
    public async Task<IActionResult> MakeUserAsAdmin(Guid id)
    {
        await _unitOfWork.ChangeUserRoleToAdmin(id);

        return Ok(new { Response = "UserRoleUpdated" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id) => await Delete(id);
}
