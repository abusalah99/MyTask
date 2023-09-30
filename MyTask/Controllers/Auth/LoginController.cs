namespace MyTask.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : BaseControllerSetting<User>
{
    private readonly IAuthUnitOfWork _unitOfWork;
    public LoginController(IAuthUnitOfWork authUnitOfWork) : base(authUnitOfWork)
            => _unitOfWork = authUnitOfWork;

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        Token token = await _unitOfWork.Login(request);


        SetCookie("AccessToken",
            token.AccessToken,
            token.AccessTokenExpiresAt);
        SetCookie("RefreshToken",
            token.RefreshToken,
            token.RefreshTokenExpiresAtExpires);

        return Ok(new {Response = token });
    }
}
