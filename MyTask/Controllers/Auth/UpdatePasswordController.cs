namespace MyTask.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class UpdatePasswordController : BaseControllerSetting<User>
{
    private readonly IAuthUnitOfWork _unitOfWork;
    public UpdatePasswordController(IAuthUnitOfWork unitOfWork) : base(unitOfWork) => _unitOfWork = unitOfWork;

    [HttpPut]
    public async Task<IActionResult> Put(PasswordRequest request)
    {
        Token token = await _unitOfWork.UpdatePassword(request);

        SetCookie("AccessToken",
        token.AccessToken,
        token.AccessTokenExpiresAt);
        SetCookie("RefreshToken",
            token.RefreshToken,
            token.RefreshTokenExpiresAtExpires);

        return Ok(token);
    }
}