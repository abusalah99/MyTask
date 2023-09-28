using Microsoft.Extensions.Hosting;

namespace MyTask.Controllers;

[Route("api/[controller]")]
[ApiController]

public class RegisterController : BaseController<User>
{
    private readonly IAuthUnitOfWork _unitOfWork;
    private readonly IHostEnvironment _hostEnvironment;
    public RegisterController(IAuthUnitOfWork unitOfWork, IHostEnvironment hostEnvironment) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }

   
    [HttpPost]
    public async Task<IActionResult> Post(UserRequest user) 
    {
        Token token = await _unitOfWork.Register(user, _hostEnvironment.ContentRootPath);

        SetCookie("AccessToken",
        token.AccessToken,
        token.AccessTokenExpiresAt);
        SetCookie("RefreshToken",
            token.RefreshToken,
            token.RefreshTokenExpiresAtExpires);

        return Ok(token);
    }

}