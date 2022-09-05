using API.Contracts.User;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.AppServices.Services.UserServices
{
    public interface IUserService
    {
        Task<LoginResponseDto> Login(LoginDto model);
    }
}
