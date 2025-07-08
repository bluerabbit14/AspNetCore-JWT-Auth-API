using Asp_.Net_Web_Api.Data;
using Asp_.Net_Web_Api.Interface;
using Asp_.Net_Web_Api.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace Asp_.Net_Web_Api.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateProfileAsync(User updatedUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updatedUser.Id);
            if (user != null)
            {
                user.FullName = updatedUser.FullName;
                user.Age = updatedUser.Age;
                user.Gender = updatedUser.Gender;
                user.AnnualIncome = updatedUser.AnnualIncome;
                user.Profession = updatedUser.Profession;
                user.State = updatedUser.State;
                await _context.SaveChangesAsync();
            }
        }
    }
}
