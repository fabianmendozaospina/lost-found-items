using LostAndFoundItems.Common;
using LostAndFoundItems.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundItems.DAL
{
    public class FoundItemRepository
    {
        private readonly LostAndFoundDbContext _context;

        public FoundItemRepository(LostAndFoundDbContext context)
        {
            _context = context;
        }

        public async Task<List<FoundItem>> GetAllFoundItems()
        {
            return await _context.FoundItems
                .Include(fi => fi.User)
                .Include(fi => fi.Location)
                .Include(fi => fi.Category)
                .ToListAsync();
        }

        public async Task<FoundItem> GetFoundItemById(int id)
        {
            return await _context.FoundItems
                .Include(fi => fi.User)
                .Include(fi => fi.Location)
                .Include(fi => fi.Category)
                .SingleOrDefaultAsync(r => r.FoundItemId == id);
        }

        public async Task<FoundItem> AddFoundItem(FoundItem foundItem)
        {
            _context.FoundItems.Add(foundItem);
            await _context.SaveChangesAsync();

            return foundItem;
        }


        public async Task UpdateFoundItem(FoundItem foundItem)
        {
            FoundItem existingFoundItem = await _context.FoundItems.FirstOrDefaultAsync(r => r.FoundItemId == foundItem.FoundItemId);

            if (existingFoundItem == null)
            {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingFoundItem.UserId = foundItem.UserId;
            existingFoundItem.LocationId = foundItem.LocationId;
            existingFoundItem.CategoryId = foundItem.CategoryId;
            existingFoundItem.Title = foundItem.Title;
            existingFoundItem.Description = foundItem.Description;
            existingFoundItem.FoundDate = foundItem.FoundDate;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteFoundItem(FoundItem foundItem)
        {
            if (foundItem != null)
            {
                _context.FoundItems.Remove(foundItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
