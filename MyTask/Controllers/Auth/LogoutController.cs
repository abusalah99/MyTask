namespace MyTask.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogoutController : BaseControllerSetting<User>
{
    private readonly IAuthUnitOfWork _unitOfWork;

    public LogoutController(IAuthUnitOfWork unitOfWork) : base(unitOfWork)
            => _unitOfWork = unitOfWork;

    [HttpPost]
    public async Task<IActionResult> Logout(RefreshTokenValue? requestToken)
    {

        string oldToken = Request.Cookies["RefreshToken"] ?? string.Empty;

        if (requestToken != null)
            oldToken = requestToken.RefreshToken;

        await _unitOfWork.Logout(oldToken);

        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddYears(-1),
            SameSite = SameSiteMode.None,
            HttpOnly = true,
            Secure = true,
        };
        Response.Cookies.Delete("AccessToken", cookieOptions);
        Response.Cookies.Delete("RefreshToken", cookieOptions);
     
        return Ok(new { Reponse = "Logout Sccess" });
    }
}
