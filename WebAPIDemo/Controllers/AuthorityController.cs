﻿using System.IdentityModel.Tokens.Jwt;
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
        if (AppRepository.Authenticate(credential.ClientId, credential.Secret))
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(10);
            return Ok(new
            {
                access_token = CreateToken(credential.ClientId, expiresAt),
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

    private string CreateToken(string clientId, DateTime expiresAt)
    {
        var app = AppRepository.GetApplicationByClientId(clientId);

        var claims = new List<Claim>
        {
            new Claim("AppName", app?.ApplicationName ?? string.Empty),
            new Claim("Read", (app?.Scopes??string.Empty).Contains("read")?"true":"false"),
            new Claim("Write", (app?.Scopes??string.Empty).Contains("write")?"true":"false"),
        };

        var secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretKey"));

        var jwt = new JwtSecurityToken(
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature
                ),
            claims: claims,
            expires: expiresAt,
            notBefore: DateTime.UtcNow
            );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}