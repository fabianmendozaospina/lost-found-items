using LostAndFoundItems.Common;
using LostAndFoundItems.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundItems.DAL
{
    public class LostItemRepository
    {
        private readonly LostAndFoundDbContext _context;

        public LostItemRepository(LostAndFoundDbContext context)
        {
            _context = context;
        }

        public async Task<List<LostItem>> GetAllLostItems()
        {
            return await _context.LostItems
                .Include(fi => fi.User)        
                .Include(fi => fi.Location)    
                .Include(fi => fi.Category)
                .ToListAsync();
        }

        public async Task<LostItem> GetLostItemById(int id)
        {
            return await _context.LostItems
                .Include(fi => fi.User)
                .Include(fi => fi.Location)
                .Include(fi => fi.Category)
                .SingleOrDefaultAsync(r => r.LostItemId == id);
        }

        public async Task<LostItem> AddLostItem(LostItem lostItem)
        {
            _context.LostItems.Add(lostItem);
            lostItem.User = await _context.Users.FindAsync(lostItem.UserId);
            lostItem.Location = await _context.Locations.FindAsync(lostItem.LocationId);
            lostItem.Category = await _context.Categories.FindAsync(lostItem.CategoryId);
            await _context.SaveChangesAsync();

            return lostItem;
        }


        public async Task UpdateLostItem(LostItem lostItem)
        {
            LostItem existingLostItem = await _context.LostItems.FirstOrDefaultAsync(r => r.LostItemId == lostItem.LostItemId);

            if (existingLostItem == null)
            {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingLostItem.UserId = lostItem.UserId;
            existingLostItem.LocationId = lostItem.LocationId;
            existingLostItem.CategoryId = lostItem.CategoryId;
            existingLostItem.Title = lostItem.Title;
            existingLostItem.Description = lostItem.Description;
            existingLostItem.LostDate = lostItem.LostDate;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteLostItem(LostItem lostItem)
        {
            if (lostItem != null)
            {
                _context.LostItems.Remove(lostItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
