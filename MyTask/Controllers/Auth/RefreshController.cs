namespace MyTask.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RefreshController : BaseController<User>
{
    private readonly IAuthUnitOfWork _unitOfWork;

    public RefreshController(IAuthUnitOfWork unitOfWork) : base(unitOfWork)
        => _unitOfWork = unitOfWork;
    
    [HttpPost]
    public async Task<IActionResult> Refresh(Token? refreshToken)
    {
        string oldToken = Request.Cookies["RefreshToken"] ?? string.Empty;

        if (refreshToken != null && refreshToken.RefreshToken != null)
            oldToken = refreshToken.RefreshToken;

        Token token = await _unitOfWork.Refresh(oldToken);

        SetCookie("AccessToken",
        token.AccessToken,
        token.AccessTokenExpiresAt);
        SetCookie("RefreshToken",
            token.RefreshToken,
            token.RefreshTokenExpiresAtExpires);

        return Ok(token);
    }

}
