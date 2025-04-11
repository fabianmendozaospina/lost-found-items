using AutoMapper;
using LostAndFoundItems.BLL;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimStatusController : ControllerBase
    {
        private readonly ClaimStatusService _claimStatusService;
        private readonly IMapper _mapper;

        public ClaimStatusController(ClaimStatusService claimStatusService, IMapper mapper)
        {
            _claimStatusService = claimStatusService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all existing claim status.
        /// </summary>
        /// <returns>A list of claim status or 404 if none exist.</returns>
        /// <response code="200">Returns the list of claim status.</response>
        /// <response code="404">No claim status found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ClaimStatusDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetClaimStatuses()
        {
            List<ClaimStatusDTO> claimStatusDTOList = await _claimStatusService.GetAllClaimStatus();

            if (claimStatusDTOList == null || claimStatusDTOList.Count == 0)
            {
                return NotFound(new ErrorResponseDTO { Error = Constants.NOT_CLAIM_STATUS_FOUND_ERROR });
            }

            return Ok(claimStatusDTOList);
        }

        /// <summary>
        /// Gets a specific claim status by its ID.
        /// </summary>
        /// <param name="id">The claim status ID.</param>
        /// <returns>The requested claim status, or 404 if not found.</returns>
        /// <response code="200">Claim status found.</response>
        /// <response code="404">Claim status not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClaimStatusDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetClaimStatusById(int id)
        {
            ClaimStatusDTO claimStatusDTO = await _claimStatusService.GetClaimStatusById(id);

            if (claimStatusDTO == null)
            {
                return NotFound(new ErrorResponseDTO { Error = $"Claim status {Constants.NOT_FOUND_ERROR}" });
            }

            return Ok(claimStatusDTO);
        }

        /// <summary>
        /// Creates a new claim status.
        /// </summary>
        /// <param name="claimStatusDTO">The claim status data to create (only name is required).</param>
        /// <returns>The created claim status with its generated ID.</returns>
        /// <response code="201">Claim status successfully created.</response>
        /// <response code="400">Invalid input or business logic error.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ClaimStatusDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddClaimStatus(ClaimStatusWriteDTO claimStatusDTO)
        {
            (ServiceResult result, ClaimStatusDTO createdClaimStatusDTO) = await _claimStatusService.AddClaimStatus(claimStatusDTO);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return CreatedAtAction(nameof(GetClaimStatusById), new { id = createdClaimStatusDTO.ClaimStatusId }, createdClaimStatusDTO);
        }

        /// <summary>
        /// Updates an existing claim status.
        /// </summary>
        /// <param name="id">The ID of the claim status to update.</param>
        /// <param name="claimStatusDTO">The new claim status data.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">Claim status updated successfully.</response>
        /// <response code="400">Invalid input or operation failed.</response>
        /// <response code="404">ClaimStatus not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateClaimStatus(int id, ClaimStatusWriteDTO claimStatusDTO)
        {
            ServiceResult result = await _claimStatusService.UpdateClaimStatus(id, claimStatusDTO);

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
        /// Deletes an existing claim status by ID.
        /// </summary>
        /// <param name="id">The ID of the claim status to delete.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">Claim status deleted successfully.</response>
        /// <response code="400">Operation failed.</response>
        /// <response code="404">ClaimStatus not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteClaimStatus(int id)
        {
            ServiceResult result = await _claimStatusService.DeleteClaimStatus(id);

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