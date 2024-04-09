using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPIDemo.Authority;

namespace WebAPIDemo.Controllers;

[ApiController]
public class AuthorityController(IConfiguration configuration) : ControllerBase
{
    [HttpPost("auth")]
    public IActionResult Authenticate([FromBody]AppCredential credential)
    {
        if (Authenticator.Authenticate(credential.ClientId, credential.Secret))
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(10);
            return Ok(new
            {
                access_token = Authenticator.CreateToken(credential.ClientId, expiresAt, configuration.GetValue<string>("SecretKey")),
                expires_at = expiresAt
            });
        }
        else
        {
            ModelState.AddModelError("Unauthorized", "You are not authorized.");
            ValidationProblemDetails problemDetails = new(ModelState)
            {
                Status = StatusCodes.Status401Unauthorized
            };
            return new UnauthorizedObjectResult(problemDetails);
        }
    }

}