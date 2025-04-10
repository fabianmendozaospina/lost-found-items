using AutoMapper;
using LostAndFoundItems.BLL;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.Models;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundItems.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase {
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(CategoryService categoryService, IMapper mapper) {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories() {
            List<Category> categories = await _categoryService.GetAllCategories();

            if (categories == null) {
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id) {
            Category? category = await _categoryService.GetCategoryById(id);

            if ((category == null)) {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDTO categoryDTO) {
            var (result, createdCategory) = await _categoryService.AddCategory(categoryDTO);

            if (!result.Success) {
                return BadRequest(new { error = result.Message });
            }

            CategoryDTO createdCategoryDTO = _mapper.Map<CategoryDTO>(createdCategory);

            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.CategoryId }, createdCategoryDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category) {
            if (id != category.CategoryId) {
                return BadRequest();
            }

            ServiceResult result = await _categoryService.UpdateCategory(category);

            if (!result.Success) {
                if (result.Code == Enums.Status.NotFound) {
                    return NotFound(new { error = (result.Message) });
                }

                return BadRequest(new { error = (result.Message) });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id) {
            Category? category = await _categoryService.GetCategoryById(id);

            if (category == null) {
                return NotFound();
            }

            await _categoryService.DeleteCategory(category);
            return NoContent();
        }
    }
}
