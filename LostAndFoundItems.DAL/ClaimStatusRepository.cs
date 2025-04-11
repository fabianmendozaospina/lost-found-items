using LostAndFoundItems.Common;
using LostAndFoundItems.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundItems.DAL
{
    public class ClaimStatusRepository
    {
        private readonly LostAndFoundDbContext _context; 
        
        public ClaimStatusRepository(LostAndFoundDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClaimStatus>> GetAllClaimStatus()
        {
            return await _context.ClaimStatuses
                .ToListAsync();
        }

        public async Task<ClaimStatus> GetClaimStatusById(int id)
        {
            return await _context.ClaimStatuses
                .SingleOrDefaultAsync(r => r.ClaimStatusId == id);
        }

        public async Task<ClaimStatus> AddClaimStatus(ClaimStatus claimStatus)
        {
            _context.ClaimStatuses.Add(claimStatus);
            await _context.SaveChangesAsync();

            return claimStatus;
        }


        public async Task UpdateClaimStatus(ClaimStatus claimStatus)
        {
            ClaimStatus existingClaimStatus = await _context.ClaimStatuses.FirstOrDefaultAsync(r => r.ClaimStatusId == claimStatus.ClaimStatusId);

            if (existingClaimStatus == null)
            {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingClaimStatus.Name = claimStatus.Name;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteClaimStatus(ClaimStatus claimStatus)
        {
            if (claimStatus != null)
            {
                _context.ClaimStatuses.Remove(claimStatus);
                await _context.SaveChangesAsync();
            }
        }
    }
}
