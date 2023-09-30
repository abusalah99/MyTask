namespace MyTask.Controllers;

[Route("api/[controller]")]
[ApiController]

public class RegisterController : BaseControllerSetting<User>
{
    private readonly IAuthUnitOfWork _unitOfWork;
    public RegisterController(IAuthUnitOfWork unitOfWork) : base(unitOfWork) => _unitOfWork = unitOfWork;

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] UserRequest user)
    {

        Token token = await _unitOfWork.Register(user);

        SetCookie("AccessToken",
        token.AccessToken,
        token.AccessTokenExpiresAt);
        SetCookie("RefreshToken",
            token.RefreshToken,
            token.RefreshTokenExpiresAtExpires);

        return Ok(new { Response = token });
    }
}
