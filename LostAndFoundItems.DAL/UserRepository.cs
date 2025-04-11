using LostAndFoundItems.Common;
using LostAndFoundItems.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundItems.DAL {
    public class UserRepository {
        private readonly LostAndFoundDbContext _context;

        public UserRepository(LostAndFoundDbContext context) {
            _context = context;
        }
        public async Task<List<User>> GetAllUsers() {
            return await _context.Users
                .Include(r => r.Role)
                .Include(c => c.ClaimRequests)
                .Include(m => m.MatchItems)
                .Include(l => l.LostItems)
                .ToListAsync();
        }

        public async Task<User> GetUserById(int id) {
            return await _context.Users
                .Include(r => r.Role)
                .Include(c => c.ClaimRequests)
                .Include(m => m.MatchItems)
                .Include(l => l.LostItems)
                .SingleOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> AddUser(User user) {
            _context.Users.Add(user);
            user.Role = await _context.Roles.FindAsync(user.RoleId);
            await _context.SaveChangesAsync();

            return user;
        }


        public async Task UpdateUser(User user) {
            User existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);

            if (existingUser == null) {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingUser.RoleId = user.RoleId;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(User user) {
            if (user != null) {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
