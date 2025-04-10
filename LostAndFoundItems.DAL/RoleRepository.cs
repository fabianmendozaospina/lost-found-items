using LostAndFoundItems.Common;
using LostAndFoundItems.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundItems.DAL
{
    public class RoleRepository
    {
        private readonly LostAndFoundDbContext _context; 
        
        public RoleRepository(LostAndFoundDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            return await _context.Roles
                .Include(r => r.Users)
                .ToListAsync();
        }

        public async Task<Role> GetRoleById(int id)
        {
            return await _context.Roles
                .Include(u => u.Users)
                .SingleOrDefaultAsync(r => r.RoleId == id);
        }

        public async Task AddRole(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRole(Role role)
        {
            Role existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == role.RoleId);

            if (existingRole == null)
            {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingRole.Name = role.Name;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRole(Role role)
        {
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }
    }
}
