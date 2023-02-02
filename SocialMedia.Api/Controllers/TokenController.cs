using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TokenController(IConfiguration configuration)
        {
            _configuration= configuration;
        }
        [HttpPost]
        public IActionResult Authentication(UserLogin login)
        {
            // if its a valid user
            if (IsValidUser(login))
            {
                var token = GenerateToken();
                return Ok(new {token});
            }
            return NotFound();
        }

        private bool IsValidUser(UserLogin login)
        {
            return true;
        }
        private string GenerateToken()
        {
            //Header
            var _symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(_symetricSecurityKey,SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,"Ausberto Medina"),
                new Claim(ClaimTypes.Email,"ausbermeca@hotmail.com"),
                new Claim(ClaimTypes.Role,"Administradors")
            };

            //Payload
            var payload = new JwtPayload
                (
                    _configuration["Authentication:Issuser"],
                    _configuration["Authentication:Audience"],
                    claims,
                    DateTime.Now,
                    DateTime.UtcNow.AddMinutes(2)
                );

            var token = new JwtSecurityToken(header,payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
