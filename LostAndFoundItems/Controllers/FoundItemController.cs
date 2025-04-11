using AutoMapper;
using LostAndFoundItems.BLL;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoundItemController : ControllerBase
    {
        private readonly FoundItemService _foundItemService;
        private readonly IMapper _mapper;

        public FoundItemController(FoundItemService foundItemService, IMapper mapper)
        {
            _foundItemService = foundItemService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all existing found items.
        /// </summary>
        /// <returns>A list of found items or 404 if none exist.</returns>
        /// <response code="200">Returns the list of found items.</response>
        /// <response code="404">No found items found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<FoundItemDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFoundItems()
        {
            List<FoundItemDTO> foundItemDTOList = await _foundItemService.GetAllFoundItems();

            if (foundItemDTOList == null || foundItemDTOList.Count == 0)
            {
                return NotFound(new ErrorResponseDTO { Error = Constants.NOT_FOUND_ITEMS_FOUND_ERROR });
            }

            return Ok(foundItemDTOList);
        }

        /// <summary>
        /// Gets a specific found item by its ID.
        /// </summary>
        /// <param name="id">The found item ID.</param>
        /// <returns>The requested found item, or 404 if not found.</returns>
        /// <response code="200">Found item found.</response>
        /// <response code="404">Found item not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FoundItemDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFoundItemById(int id)
        {
            FoundItemDTO foundItemDTO = await _foundItemService.GetFoundItemById(id);

            if (foundItemDTO == null)
            {
                return NotFound(new ErrorResponseDTO { Error = $"Found item {Constants.NOT_FOUND_ERROR}" });
            }

            return Ok(foundItemDTO);
        }

        /// <summary>
        /// Creates a new found item.
        /// </summary>
        /// <param name="foundItemDTO">The found item data to create (only name is required).</param>
        /// <returns>The created found item with its generated ID.</returns>
        /// <response code="201">Found item successfully created.</response>
        /// <response code="400">Invalid input or business logic error.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(FoundItemDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddFoundItem(FoundItemWriteDTO foundItemDTO)
        {
            (ServiceResult result, FoundItemDTO? createdFoundItemDTO) = await _foundItemService.AddFoundItem(foundItemDTO);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return CreatedAtAction(nameof(GetFoundItemById), new { id = createdFoundItemDTO?.FoundItemId }, createdFoundItemDTO);
        }

        /// <summary>
        /// Updates an existing found item.
        /// </summary>
        /// <param name="id">The ID of the found item to update.</param>
        /// <param name="foundItemDTO">The new found item data.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">Found item updated successfully.</response>
        /// <response code="400">Invalid input or operation failed.</response>
        /// <response code="404">FoundItem not found.</response>
        /// <response code="500">Unexpected server error.</response>        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFoundItem(int id, FoundItemWriteDTO foundItemDTO)
        {
            ServiceResult result = await _foundItemService.UpdateFoundItem(id, foundItemDTO);

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
        /// Deletes an existing found item by ID.
        /// </summary>
        /// <param name="id">The ID of the found item to delete.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">Found item deleted successfully.</response>
        /// <response code="400">Operation failed.</response>
        /// <response code="404">FoundItem not found.</response>
        /// <response code="500">Unexpected server error.</response>        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFoundItem(int id)
        {
            ServiceResult result = await _foundItemService.DeleteFoundItem(id);

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