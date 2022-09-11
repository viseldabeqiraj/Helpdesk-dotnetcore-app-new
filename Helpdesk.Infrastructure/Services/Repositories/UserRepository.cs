using Helpdesk.Core.Dtos.User;
using Helpdesk.Core.Interfaces;
using Helpdesk.Infrastructure.Data;
using Helpdesk.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Helpdesk.Infrastructure.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddUserAsync(RegisterDto registerDto)
        {
            var toAdd = new User
            {
                Username = registerDto.Username,
                Password = registerDto.Password
            };

            _dbContext.User.Add(toAdd);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<UserDto> GetUserAsync(LoginDto loginDto)
        {
            var user = await _dbContext
                .User
                .Where(u => u.Username == loginDto.Username && u.Password == loginDto.Password)
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync();

            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                Roles = user.UserRoles
                    .Select(ur => ur.Role.Title)
                    .ToList(),
                Surname = user.Surname
            };
        }

        public async Task<bool> UserExistsForRegistrationAsync(string username)
        {
            var exists = await _dbContext
                .User
                .AnyAsync(u => u.Username == username);

            return exists;
        }
    }
}
