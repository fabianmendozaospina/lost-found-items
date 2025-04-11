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
        /// Gets all existing foundItems.
        /// </summary>
        /// <returns>A list of foundItems or 404 if none exist.</returns>
        /// <response code="200">Returns the list of foundItems.</response>
        /// <response code="404">No foundItems found.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<FoundItemDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFoundItems()
        {
            List<FoundItemDTO> foundItemDTOList = await _foundItemService.GetAllFoundItems();

            if (foundItemDTOList == null || foundItemDTOList.Count == 0)
            {
                return NotFound(new ErrorResponseDTO { Error = Constants.NOT_ROLES_FOUND_ERROR });
            }

            return Ok(foundItemDTOList);
        }

        /// <summary>
        /// Gets a specific foundItem by its ID.
        /// </summary>
        /// <param name="id">The foundItem ID.</param>
        /// <returns>The requested foundItem, or 404 if not found.</returns>
        /// <response code="200">FoundItem found.</response>
        /// <response code="404">FoundItem not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FoundItemDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFoundItemById(int id)
        {
            FoundItemDTO foundItemDTO = await _foundItemService.GetFoundItemById(id);

            if (foundItemDTO == null)
            {
                return NotFound(new ErrorResponseDTO { Error = $"FoundItem {Constants.NOT_FOUND_ERROR}" });
            }

            return Ok(foundItemDTO);
        }

        /// <summary>
        /// Creates a new foundItem.
        /// </summary>
        /// <param name="foundItemDTO">The foundItem data to create (only name is required).</param>
        /// <returns>The created foundItem with its generated ID.</returns>
        /// <response code="201">FoundItem successfully created.</response>
        /// <response code="400">Invalid input or business logic error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(FoundItemDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
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
        /// Updates an existing foundItem.
        /// </summary>
        /// <param name="id">The ID of the foundItem to update.</param>
        /// <param name="foundItemDTO">The new foundItem data.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">FoundItem updated successfully.</response>
        /// <response code="400">Invalid input or operation failed.</response>
        /// <response code="404">FoundItem not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
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
        /// Deletes an existing foundItem by ID.
        /// </summary>
        /// <param name="id">The ID of the foundItem to delete.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">FoundItem deleted successfully.</response>
        /// <response code="400">Operation failed.</response>
        /// <response code="404">FoundItem not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
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