using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    [HttpGet]
    [Authorize(AuthenticationSchemes = "Negotiate")]
    public IActionResult Get()
    {
 var identity = HttpContext.User.Identity;
        if (identity is WindowsIdentity windowsIdentity)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            var username = windowsIdentity.Name; // DOMAIN\username
#pragma warning restore CA1416 // Validate platform compatibility
            return Ok(new { username });
        }

        return Unauthorized();
    }
}