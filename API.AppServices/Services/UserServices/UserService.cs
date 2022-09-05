using API.Contracts.User;
using API.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.AppServices.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IConfiguration config, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<LoginResponseDto> Login(LoginDto model)
        {
            if(model == null)
            {
                throw new Exception("Неверные данные для входа");
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user == null)
            {
                throw new Exception("Неверные данные для входа");
            }
            if(await _userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new Exception("Неверный пароль");
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.UserName) };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Secret:Key"]));
            var credentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            return new LoginResponseDto()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.ToArray(),
                Token = handler.WriteToken(token)
            };
        }
    }
}
