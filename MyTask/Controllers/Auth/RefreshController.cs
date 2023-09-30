namespace MyTask.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RefreshController : BaseControllerSetting<User>
{
    private readonly IAuthUnitOfWork _unitOfWork;

    public RefreshController(IAuthUnitOfWork unitOfWork) : base(unitOfWork)
        => _unitOfWork = unitOfWork;

    [HttpPost]
    public async Task<IActionResult> Refresh(RefreshTokenValue? requestToken)
    {

        string oldToken = Request.Cookies["RefreshToken"] ?? string.Empty;

        if (requestToken != null)
            oldToken = requestToken.RefreshToken;

        Token token = await _unitOfWork.Refresh(oldToken);

        SetCookie("AccessToken",
        token.AccessToken,
        token.AccessTokenExpiresAt);
        SetCookie("RefreshToken",
            token.RefreshToken,
            token.RefreshTokenExpiresAtExpires);

        return Ok(new { Response = token });
    }
}
