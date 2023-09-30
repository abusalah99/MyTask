namespace MyTask.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : BaseControllerSetting<User>
{
    private readonly IUserUnitOfWork _unitOfWork;
    public UserController(IUserUnitOfWork unitOfWork) : base(unitOfWork) => _unitOfWork = unitOfWork;

    [HttpGet]
    public async Task<IActionResult> Get() => await Read(GetUserId());

    [HttpGet, Route("ProfilePhoto")]
    public async Task<IActionResult> GetProfilePhoto() => File(await _unitOfWork.GetUserImage(), "image/jpeg");

    [HttpPut]
    public async Task<IActionResult> Put(UserRequest request)
    {
        User user = await _unitOfWork.MapFromUserRequestToUser(request);

        await _unitOfWork.Update(user);

        return Ok(new { Reponse = $"User Updated" });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        await _unitOfWork.Delete(GetUserId());
        
        return Ok(new { Reponse = $"User Updated" });
    }
    
}
