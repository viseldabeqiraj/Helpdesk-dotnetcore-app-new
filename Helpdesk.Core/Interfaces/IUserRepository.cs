using Helpdesk.Core.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto> GetUserAsync(LoginDto loginDto);
        Task<bool> UserExistsForRegistrationAsync(string username);
        Task<bool> AddUserAsync(RegisterDto registerDto);
    }
}
