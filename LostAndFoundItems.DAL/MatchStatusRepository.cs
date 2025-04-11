using LostAndFoundItems.Common;
using LostAndFoundItems.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundItems.DAL
{
    public class MatchStatusRepository
    {
        private readonly LostAndFoundDbContext _context; 
        
        public MatchStatusRepository(LostAndFoundDbContext context)
        {
            _context = context;
        }

        public async Task<List<MatchStatus>> GetAllMatchStatus()
        {
            return await _context.MatchStatuses
                .ToListAsync();
        }

        public async Task<MatchStatus> GetMatchStatusById(int id)
        {
            return await _context.MatchStatuses
                .SingleOrDefaultAsync(r => r.MatchStatusId == id);
        }

        public async Task<MatchStatus> AddMatchStatus(MatchStatus claimStatus)
        {
            _context.MatchStatuses.Add(claimStatus);
            await _context.SaveChangesAsync();

            return claimStatus;
        }


        public async Task UpdateMatchStatus(MatchStatus claimStatus)
        {
            MatchStatus existingMatchStatus = await _context.MatchStatuses.FirstOrDefaultAsync(r => r.MatchStatusId == claimStatus.MatchStatusId);

            if (existingMatchStatus == null)
            {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingMatchStatus.Name = claimStatus.Name;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteMatchStatus(MatchStatus claimStatus)
        {
            if (claimStatus != null)
            {
                _context.MatchStatuses.Remove(claimStatus);
                await _context.SaveChangesAsync();
            }
        }
    }
}
