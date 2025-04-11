using AutoMapper;
using LostAndFoundItems.BLL;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimRequestController : ControllerBase
    {
        private readonly ClaimRequestService _claimRequestService;
        private readonly IMapper _mapper;

        public ClaimRequestController(ClaimRequestService claimRequestService, IMapper mapper)
        {
            _claimRequestService = claimRequestService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all existing claim requests.
        /// </summary>
        /// <returns>A list of claim requests or 404 if none exist.</returns>
        /// <response code="200">Returns the list of claim requests.</response>
        /// <response code="404">No claim requests found.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ClaimRequestDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClaimRequests()
        {
            List<ClaimRequestDTO> claimRequestDTOList = await _claimRequestService.GetAllClaimRequests();

            if (claimRequestDTOList == null || claimRequestDTOList.Count == 0)
            {
                return NotFound(new ErrorResponseDTO { Error = Constants.NOT_CLAIM_RECUEST_FOUND_ERROR });
            }

            return Ok(claimRequestDTOList);
        }

        /// <summary>
        /// Gets a specific claim request by its ID.
        /// </summary>
        /// <param name="id">The claim request ID.</param>
        /// <returns>The requested claim request, or 404 if not found.</returns>
        /// <response code="200">Claim request found.</response>
        /// <response code="404">Claim request not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClaimRequestDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClaimRequestById(int id)
        {
            ClaimRequestDTO claimRequestDTO = await _claimRequestService.GetClaimRequestById(id);

            if (claimRequestDTO == null)
            {
                return NotFound(new ErrorResponseDTO { Error = $"Claim request {Constants.NOT_FOUND_ERROR}" });
            }

            return Ok(claimRequestDTO);
        }

        /// <summary>
        /// Creates a new claim request.
        /// </summary>
        /// <param name="claimRequestDTO">The claim request data to create (only name is required).</param>
        /// <returns>The created claim request with its generated ID.</returns>
        /// <response code="201">Claim request successfully created.</response>
        /// <response code="400">Invalid input or business logic error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ClaimRequestDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddClaimRequest(ClaimRequestWriteDTO claimRequestDTO)
        {
            (ServiceResult result, ClaimRequestDTO createdClaimRequestDTO) = await _claimRequestService.AddClaimRequest(claimRequestDTO);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return CreatedAtAction(nameof(GetClaimRequestById), new { id = createdClaimRequestDTO.ClaimRequestId }, createdClaimRequestDTO);
        }

        /// <summary>
        /// Updates an existing claim request.
        /// </summary>
        /// <param name="id">The ID of the claim request to update.</param>
        /// <param name="claimRequestDTO">The new claim request data.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">Claim request updated successfully.</response>
        /// <response code="400">Invalid input or operation failed.</response>
        /// <response code="404">Claim request not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateClaimRequest(int id, ClaimRequestWriteDTO claimRequestDTO)
        {
            ServiceResult result = await _claimRequestService.UpdateClaimRequest(id, claimRequestDTO);

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
        /// Deletes an existing claim request by ID.
        /// </summary>
        /// <param name="id">The ID of the claim request to delete.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">Claim request deleted successfully.</response>
        /// <response code="400">Operation failed.</response>
        /// <response code="404">Claim request not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteClaimRequest(int id)
        {
            ServiceResult result = await _claimRequestService.DeleteClaimRequest(id);

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