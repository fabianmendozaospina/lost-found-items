using AutoMapper;
using LostAndFoundItems.BLL;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.Common;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundItems.Controllers
{
    public class MatchsStatusController : ControllerBase
    {
        private readonly MatchStatusService _matchStatusService;
        private readonly IMapper _mapper;

        public MatchsStatusController(MatchStatusService matchStatusService, IMapper mapper)
        {
            _matchStatusService = matchStatusService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all existing match status.
        /// </summary>
        /// <returns>A list of match status or 404 if none exist.</returns>
        /// <response code="200">Returns the list of match statuses.</response>
        /// <response code="404">No match status found.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<MatchStatusDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMatchStatuses()
        {
            List<MatchStatusDTO> matchStatusDTOList = await _matchStatusService.GetAllMatchStatus();

            if (matchStatusDTOList == null || matchStatusDTOList.Count == 0)
            {
                return NotFound(new ErrorResponseDTO { Error = Constants.NOT_MATCH_STATUS_FOUND_ERROR });
            }

            return Ok(matchStatusDTOList);
        }

        /// <summary>
        /// Gets a specific match status by its ID.
        /// </summary>
        /// <param name="id">The match status ID.</param>
        /// <returns>The requested match status, or 404 if not found.</returns>
        /// <response code="200">Match status found.</response>
        /// <response code="404">Match status not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MatchStatusDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMatchStatusById(int id)
        {
            MatchStatusDTO matchStatusDTO = await _matchStatusService.GetMatchStatusById(id);

            if (matchStatusDTO == null)
            {
                return NotFound(new ErrorResponseDTO { Error = $"Match status {Constants.NOT_FOUND_ERROR}" });
            }

            return Ok(matchStatusDTO);
        }

        /// <summary>
        /// Creates a new match status.
        /// </summary>
        /// <param name="matchStatusDTO">The match status data to create (only name is required).</param>
        /// <returns>The created match status with its generated ID.</returns>
        /// <response code="201">Match status successfully created.</response>
        /// <response code="400">Invalid input or business logic error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(MatchStatusDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMatchStatus(MatchStatusWriteDTO matchStatusDTO)
        {
            (ServiceResult result, MatchStatusDTO createdMatchStatusDTO) = await _matchStatusService.AddMatchStatus(matchStatusDTO);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return CreatedAtAction(nameof(GetMatchStatusById), new { id = createdMatchStatusDTO.MatchStatusId }, createdMatchStatusDTO);
        }

        /// <summary>
        /// Updates an existing match status.
        /// </summary>
        /// <param name="id">The ID of the match status to update.</param>
        /// <param name="matchStatusDTO">The new match status data.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">Match status updated successfully.</response>
        /// <response code="400">Invalid input or operation failed.</response>
        /// <response code="404">Match status not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMatchStatus(int id, MatchStatusWriteDTO matchStatusDTO)
        {
            ServiceResult result = await _matchStatusService.UpdateMatchStatus(id, matchStatusDTO);

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
        /// Deletes an existing match status by ID.
        /// </summary>
        /// <param name="id">The ID of the match status to delete.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">Match status deleted successfully.</response>
        /// <response code="400">Operation failed.</response>
        /// <response code="404">MatchStatus not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMatchStatus(int id)
        {
            ServiceResult result = await _matchStatusService.DeleteMatchStatus(id);

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
