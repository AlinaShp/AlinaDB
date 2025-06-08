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
            var username = windowsIdentity.Name; // DOMAIN\username
            return Ok(new { username });
        }

        return Unauthorized();
    }
}