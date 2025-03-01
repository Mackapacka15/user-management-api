namespace User_Management.Controllers;

using Microsoft.AspNetCore.Mvc;
using User_Management.Services;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService =
        authService ?? throw new ArgumentNullException(nameof(authService));
    private static readonly string MasterPassword = "YourStrongMasterPassword"; // should store in a secure place but for this it's fine

    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] AuthRequest request)
    {
        if (request.MasterKey != MasterPassword)
        {
            return Unauthorized(new { message = "Invalid master key" });
        }

        var token = _authService.GenerateToken();
        return Ok(new { token });
    }
}

public class AuthRequest
{
    public required string MasterKey { get; set; }
}
