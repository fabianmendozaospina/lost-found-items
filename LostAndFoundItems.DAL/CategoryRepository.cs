using LostAndFoundItems.Common;
using LostAndFoundItems.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundItems.DAL 
{
    public class CategoryRepository 
    {
        public readonly LostAndFoundDbContext _context;

        public CategoryRepository(LostAndFoundDbContext context) 
        {
            _context = context;
        }
        public async Task<List<Category>> GetAllCategories() 
        {
            return await _context.Categories
                .Include(l => l.FoundItems)
                .Include(l => l.LostItems)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id) 
        {
            return await _context.Categories
                .Include(l => l.FoundItems)
                .Include(l => l.LostItems)
                .SingleOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<Category> AddCategory(Category category) 
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task UpdateCategory(Category category) 
        {
            Category existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);

            if (existingCategory == null) 
            {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingCategory.Name = category.Name;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(Category category) 
        {
            if (category != null) 
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
