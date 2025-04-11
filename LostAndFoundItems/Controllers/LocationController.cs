using AutoMapper;
using LostAndFoundItems.BLL;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly LocationService _locationService;
        private readonly IMapper _mapper;

        public LocationController(LocationService locationService, IMapper mapper)
        {
            _locationService = locationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all existing locations.
        /// </summary>
        /// <returns>A list of locations or 404 if none exist.</returns>
        /// <response code="200">Returns the list of locations.</response>
        /// <response code="404">No locations found.</response>
        /// <response code="500">Unexpected server error.</response>        
        [HttpGet]
        [ProducesResponseType(typeof(List<LocationDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLocations()
        {
            List<LocationDTO> locationDTOList = await _locationService.GetAllLocations();

            if (locationDTOList == null || locationDTOList.Count == 0)
            {
                return NotFound(new ErrorResponseDTO { Error = Constants.NOT_LOCATIONS_FOUND_ERROR });
            }

            return Ok(locationDTOList);
        }

        /// <summary>
        /// Gets a specific location by its ID.
        /// </summary>
        /// <param name="id">The location ID.</param>
        /// <returns>The requested location, or 404 if not found.</returns>
        /// <response code="200">Location found.</response>
        /// <response code="404">Location not found.</response>
        /// <response code="500">Unexpected server error.</response>        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LocationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLocationById(int id)
        {
            LocationDTO locationDTO = await _locationService.GetLocationById(id);

            if (locationDTO == null)
            {
                return NotFound(new ErrorResponseDTO { Error = $"Location {Constants.NOT_FOUND_ERROR}" });
            }

            return Ok(locationDTO);
        }

        /// <summary>
        /// Creates a new location.
        /// </summary>
        /// <param name="locationDTO">The location data to create (only name is required).</param>
        /// <returns>The created location with its generated ID.</returns>
        /// <response code="201">Location successfully created.</response>
        /// <response code="400">Invalid input or business logic error.</response>
        /// <response code="500">Unexpected server error.</response>        
        [HttpPost]
        [ProducesResponseType(typeof(LocationDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddLocation(LocationWriteDTO locationDTO)
        {
            (ServiceResult result, LocationDTO createdLocationDTO) = await _locationService.AddLocation(locationDTO);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return CreatedAtAction(nameof(GetLocationById), new { id = createdLocationDTO.LocationId }, createdLocationDTO);
        }

        /// <summary>
        /// Updates an existing location.
        /// </summary>
        /// <param name="id">The ID of the location to update.</param>
        /// <param name="locationDTO">The new location data.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">Location updated successfully.</response>
        /// <response code="400">Invalid input or operation failed.</response>
        /// <response code="404">Location not found.</response>
        /// <response code="500">Unexpected server error.</response> 
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLocation(int id, LocationWriteDTO locationDTO)
        {
            ServiceResult result = await _locationService.UpdateLocation(id, locationDTO);

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
        /// Deletes an existing location by ID.
        /// </summary>
        /// <param name="id">The ID of the location to delete.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">Location deleted successfully.</response>
        /// <response code="400">Operation failed.</response>
        /// <response code="404">Location not found.</response>
        /// <response code="500">Unexpected server error.</response> 
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            ServiceResult result = await _locationService.DeleteLocation(id);

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