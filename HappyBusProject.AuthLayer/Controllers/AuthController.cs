using HappyBusProject.AuthLayer.Common;
using HappyBusProject.AuthLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace HappyBusProject.AuthLayer.Controllers
{
    [Route("AppAPI/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOptions<AuthOptions> authOptions;

        public AuthController(IOptions<AuthOptions> authOptions)
        {
            this.authOptions = authOptions;
        }

        private static List<Account> Accounts => new()
        {
            new Account()
            {
                Id = Guid.Parse("F5B6028E-A8AB-459D-96F9-99F0822925C3"),
                Login = "user1@gmail.com",
                Password = "user1",
                Roles = new Role[] { Role.User }
            },

            new Account()
            {
                Id = Guid.Parse("CE223A90-937C-4A58-BF94-93E6DE196C11"),
                Login = "user2@gmail.com",
                Password = "user2",
                Roles = new Role[] { Role.Driver }
            },

            new Account()
            {
                Id = Guid.Parse("DAD19788-AF76-419F-A4D1-BC047FE1A2B5"),
                Login = "admin@gmail.com",
                Password = "admin",
                Roles = new Role[] { Role.Admin }
            }
        };

        [Route("Login")]
        [HttpPost]
        public IActionResult Login([FromBody] Login request)
        {
            var user = AuthenticateUser(request.UserLogin, request.Password);

            if (user != null)
            {
                var token = GenerateJWT(user);

                return Ok(new { access_token = token });
            }

            return Unauthorized();
        }

        private static Account AuthenticateUser(string userLogin, string password)
        {
            return Accounts.SingleOrDefault(u => u.Login == userLogin && u.Password == password);
        }

        private string GenerateJWT(Account user)
        {
            try
            {
                var authParams = authOptions.Value;

                var securityKey = authParams.GetSymmetricSecurityKey();
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Name, user.Login),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }

                var token = new JwtSecurityToken
                (
                    authParams.Issuer,
                    authParams.Audience,
                    claims,
                    expires: DateTime.Now.AddSeconds(authParams.TokenLifetime),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
