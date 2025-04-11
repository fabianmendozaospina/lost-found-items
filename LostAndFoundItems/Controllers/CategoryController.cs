using AutoMapper;
using LostAndFoundItems.BLL;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase {
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(CategoryService categoryService, IMapper mapper) {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all existing categories.
        /// </summary>
        /// <returns>A list of categories or 404 if none exist.</returns>
        /// <response code="200">Returns the list of categories.</response>
        /// <response code="404">No categories found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<CategoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories() {
            List<CategoryDTO> categoryDTOList = await _categoryService.GetAllCategories();

            if (categoryDTOList == null || categoryDTOList.Count == 0) {
                return NotFound(new ErrorResponseDTO { Error = Constants.NOT_CATEGORIES_FOUND_ERROR });
            }

            return Ok(categoryDTOList);
        }

        /// <summary>
        /// Gets a specific category by its ID.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>The requested category, or 404 if not found.</returns>
        /// <response code="200">Category found.</response>
        /// <response code="404">Category not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoryById(int id) {
            CategoryDTO categoryDTO = await _categoryService.GetCategoryById(id);

            if ((categoryDTO == null)) {
                return NotFound(new ErrorResponseDTO { Error = $"Category {Constants.NOT_FOUND_ERROR}" });
            }

            return Ok(categoryDTO);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryDTO">The category data to create (only name is required).</param>
        /// <returns>The created category with its generated ID.</returns>
        /// <response code="201">Category successfully created.</response>
        /// <response code="400">Invalid input or business logic error.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCategory(CategoryWriteDTO categoryDTO) 
        {
            (ServiceResult result, CategoryDTO createdCategoryDTO) = await _categoryService.AddCategory(categoryDTO);

            if (!result.Success) 
            {
                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }


            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategoryDTO.CategoryId }, createdCategoryDTO);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="categoryDTO">The new category data.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">Category updated successfully.</response>
        /// <response code="400">Invalid input or operation failed.</response>
        /// <response code="404">Category not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategory(int id, CategoryWriteDTO categoryDTO) {
            ServiceResult result = await _categoryService.UpdateCategory(id, categoryDTO);

            if (!result.Success)
            {
                if (result.Code == Enums.Status.NotFound)
                {
                    return NotFound(new ErrorResponseDTO { Error = result.Message });
                }

                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes an existing category by ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">Category deleted successfully.</response>
        /// <response code="400">Operation failed.</response>
        /// <response code="404">Category not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            ServiceResult result = await _categoryService.DeleteCategory(id);

            if (!result.Success)
            {
                if (result.Code == Enums.Status.NotFound)
                {
                    return NotFound(new ErrorResponseDTO { Error = result.Message });
                }

                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return NoContent();
        }
    }
}
