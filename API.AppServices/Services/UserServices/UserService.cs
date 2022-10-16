using API.Contracts.User;
using API.Domain.Entities;
using API.Infrastructure.Repository;
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
        private readonly IRepository<UserProfile> _upRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IConfiguration config,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            IRepository<UserProfile> upRepository)
        {
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
            _upRepository = upRepository;
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
            if(!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new Exception("Неверный пароль");
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.UserName) };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
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

        public async Task Register(RegisterDto model)
        {
            if(model.Password != model.PasswordConfirm)
            {
                throw new Exception("Пароли не совпадают");
            }

            if(await _userManager.FindByEmailAsync(model.Email) != null)
            {
                throw new Exception("Пользователь с такой почтой уже существует");
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.UserName
            };

            UserProfile userProfile = new UserProfile()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                isDelete = false,
                Gender = model.Gender,
                User = user
            };

            
            await _userManager.CreateAsync(user, model.Password);
            await _upRepository.AddAsync(userProfile);

            var emailComfirmation = await _userManager.GenerateEmailConfirmationTokenAsync(user);


        }
    }
}
