using LostAndFoundItems.Common;
using LostAndFoundItems.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundItems.DAL
{
    public class MatchItemRepository
    {
        private readonly LostAndFoundDbContext _context;

        public MatchItemRepository(LostAndFoundDbContext context)
        {
            _context = context;
        }

        public async Task<List<MatchItem>> GetAllMatchItems()
        {
            return await _context.MatchItems
                .Include(mi => mi.User)        
                .Include(mi => mi.LostItem)    
                .Include(mi => mi.FoundItem)
                .Include(mi => mi.MatchStatus)
                .ToListAsync();
        }

        public async Task<MatchItem> GetMatchItemById(int id)
        {
            return await _context.MatchItems
                .Include(mi => mi.User)
                .Include(mi => mi.LostItem)
                .Include(mi => mi.FoundItem)
                .Include(mi => mi.MatchStatus)
                .SingleOrDefaultAsync(r => r.MatchItemId == id);
        }

        public async Task<MatchItem> AddMatchItem(MatchItem matchItem)
        {
            _context.MatchItems.Add(matchItem);
            matchItem.User = await _context.Users.FindAsync(matchItem.MatchUserId);
            matchItem.LostItem = await _context.LostItems.FindAsync(matchItem.LostItemId);
            matchItem.FoundItem = await _context.FoundItems.FindAsync(matchItem.FoundItemId);
            matchItem.MatchStatus = await _context.MatchStatuses.FindAsync(matchItem.MatchStatusId);
            await _context.SaveChangesAsync();

            return matchItem;
        }

        public async Task UpdateMatchItem(MatchItem matchItem)
        {
            MatchItem existingMatchItem = await _context.MatchItems.FirstOrDefaultAsync(r => r.MatchItemId == matchItem.MatchItemId);

            if (existingMatchItem == null)
            {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingMatchItem.MatchUserId = matchItem.MatchUserId;
            existingMatchItem.LostItemId = matchItem.LostItemId;
            existingMatchItem.FoundItemId = matchItem.FoundItemId;
            existingMatchItem.MatchStatusId = matchItem.MatchStatusId;
            existingMatchItem.Observation = matchItem.Observation;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteMatchItem(MatchItem matchItem)
        {
            if (matchItem != null)
            {
                _context.MatchItems.Remove(matchItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
