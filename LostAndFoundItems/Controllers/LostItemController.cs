using AutoMapper;
using LostAndFoundItems.BLL;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LostItemController : ControllerBase
    {
        private readonly LostItemService _lostItemService;
        private readonly IMapper _mapper;

        public LostItemController(LostItemService lostItemService, IMapper mapper)
        {
            _lostItemService = lostItemService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all existing lost items.
        /// </summary>
        /// <returns>A list of lostItems or 404 if none exist.</returns>
        /// <response code="200">Returns the list of lost items.</response>
        /// <response code="404">No lost items found.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<LostItemDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLostItems()
        {
            List<LostItemDTO> LostItemDTOList = await _lostItemService.GetAllLostItems();

            if (LostItemDTOList == null || LostItemDTOList.Count == 0)
            {
                return NotFound(new ErrorResponseDTO { Error = Constants.NOT_LOST_ITEMS_FOUND_ERROR });
            }

            return Ok(LostItemDTOList);
        }

        /// <summary>
        /// Gets a specific lost item by its ID.
        /// </summary>
        /// <param name="id">The lost item ID.</param>
        /// <returns>The requested lost item, or 404 if not found.</returns>
        /// <response code="200">lost item found.</response>
        /// <response code="404">lost item not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LostItemDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLostItemById(int id)
        {
            LostItemDTO LostItemDTO = await _lostItemService.GetLostItemById(id);

            if (LostItemDTO == null)
            {
                return NotFound(new ErrorResponseDTO { Error = $"Lost item {Constants.NOT_FOUND_ERROR}" });
            }

            return Ok(LostItemDTO);
        }

        /// <summary>
        /// Creates a new lost item.
        /// </summary>
        /// <param name="lostItemDTO">The lost item data to create (only name is required).</param>
        /// <returns>The created lost item with its generated ID.</returns>
        /// <response code="201">lost item successfully created.</response>
        /// <response code="400">Invalid input or business logic error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(LostItemDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddLostItem(LostItemWriteDTO lostItemDTO)
        {
            (ServiceResult result, LostItemDTO? createdLostItemDTO) = await _lostItemService.AddLostItem(lostItemDTO);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return CreatedAtAction(nameof(GetLostItemById), new { id = createdLostItemDTO?.LostItemId }, createdLostItemDTO);
        }

        /// <summary>
        /// Updates an existing lost item.
        /// </summary>
        /// <param name="id">The ID of the lost item to update.</param>
        /// <param name="LostItemDTO">The new lost item data.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">lost item updated successfully.</response>
        /// <response code="400">Invalid input or operation failed.</response>
        /// <response code="404">LostItem not lost.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLostItem(int id, LostItemWriteDTO LostItemDTO)
        {
            ServiceResult result = await _lostItemService.UpdateLostItem(id, LostItemDTO);

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
        /// Deletes an existing lost item by ID.
        /// </summary>
        /// <param name="id">The ID of the lost item to delete.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">lost item deleted successfully.</response>
        /// <response code="400">Operation failed.</response>
        /// <response code="404">lost item not lost.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLostItem(int id)
        {
            ServiceResult result = await _lostItemService.DeleteLostItem(id);

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