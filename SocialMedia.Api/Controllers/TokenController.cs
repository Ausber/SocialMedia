using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infraestructure.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;
        private readonly IPasswordService _passwordService;
        public TokenController(IConfiguration configuration, ISecurityService securityService, IPasswordService passwordService)
        {
            _configuration = configuration;
            _securityService = securityService;
            _passwordService = passwordService;
        }
        [HttpPost]
        public async Task<IActionResult> Authentication(UserLogin login)
        {
            // if its a valid user
            var validation = await IsValidUser(login);
            if (validation.Item1)
            {
                var token = GenerateToken(validation.Item2);
                return Ok(new {token});
            }
            return NotFound();
        }

        private async Task<(bool,Security)> IsValidUser(UserLogin login)
        {
            var user = await _securityService.GetLoginByCredentials(login);
            var IsValid = _passwordService.Check(user.Password,login.Password);
            return (IsValid,user);
        }
        private string GenerateToken(Security security)
        {
            //Header
            var _symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(_symetricSecurityKey,SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,security.UserName),
                new Claim("user",security.User),
                new Claim(ClaimTypes.Role,security.Role.ToString())
            };

            //Payload
            var payload = new JwtPayload
                (
                    _configuration["Authentication:Issuser"],
                    _configuration["Authentication:Audience"],
                    claims,
                    DateTime.Now,
                    DateTime.UtcNow.AddMinutes(10)
                );

            var token = new JwtSecurityToken(header,payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
