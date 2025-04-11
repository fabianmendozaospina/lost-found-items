using AutoMapper;
using LostAndFoundItems.BLL;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchItemController : ControllerBase
    {
        private readonly MatchItemService _matchItemService;
        private readonly IMapper _mapper;

        public MatchItemController(MatchItemService matchItemService, IMapper mapper)
        {
            _matchItemService = matchItemService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all existing match items.
        /// </summary>
        /// <returns>A list of matchItems or 404 if none exist.</returns>
        /// <response code="200">Returns the list of match items.</response>
        /// <response code="404">No match items found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<MatchItemDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMatchItems()
        {
            List<MatchItemDTO> MatchItemDTOList = await _matchItemService.GetAllMatchItems();

            if (MatchItemDTOList == null || MatchItemDTOList.Count == 0)
            {
                return NotFound(new ErrorResponseDTO { Error = Constants.NOT_MATCH_ITEMS_FOUND_ERROR });
            }

            return Ok(MatchItemDTOList);
        }

        /// <summary>
        /// Gets a specific match item by its ID.
        /// </summary>
        /// <param name="id">The match item ID.</param>
        /// <returns>The requested match item, or 404 if not found.</returns>
        /// <response code="200">match item found.</response>
        /// <response code="404">match item not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MatchItemDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMatchItemById(int id)
        {
            MatchItemDTO MatchItemDTO = await _matchItemService.GetMatchItemById(id);

            if (MatchItemDTO == null)
            {
                return NotFound(new ErrorResponseDTO { Error = $"Match item {Constants.NOT_FOUND_ERROR}" });
            }

            return Ok(MatchItemDTO);
        }

        /// <summary>
        /// Creates a new match item.
        /// </summary>
        /// <param name="matchItemDTO">The match item data to create (only name is required).</param>
        /// <returns>The created match item with its generated ID.</returns>
        /// <response code="201">match item successfully created.</response>
        /// <response code="400">Invalid input or business logic error.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(MatchItemDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMatchItem(MatchItemWriteDTO matchItemDTO)
        {
            (ServiceResult result, MatchItemDTO? createdMatchItemDTO) = await _matchItemService.AddMatchItem(matchItemDTO);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return CreatedAtAction(nameof(GetMatchItemById), new { id = createdMatchItemDTO?.MatchItemId }, createdMatchItemDTO);
        }

        /// <summary>
        /// Updates an existing match item.
        /// </summary>
        /// <param name="id">The ID of the match item to update.</param>
        /// <param name="MatchItemDTO">The new match item data.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">match item updated successfully.</response>
        /// <response code="400">Invalid input or operation failed.</response>
        /// <response code="404">MatchItem not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMatchItem(int id, MatchItemWriteDTO MatchItemDTO)
        {
            ServiceResult result = await _matchItemService.UpdateMatchItem(id, MatchItemDTO);

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
        /// Deletes an existing match item by ID.
        /// </summary>
        /// <param name="id">The ID of the match item to delete.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">match item deleted successfully.</response>
        /// <response code="400">Operation failed.</response>
        /// <response code="404">match item not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMatchItem(int id)
        {
            ServiceResult result = await _matchItemService.DeleteMatchItem(id);

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