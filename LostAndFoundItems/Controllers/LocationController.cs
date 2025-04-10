using AutoMapper;
using LostAndFoundItems.BLL;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.Models;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundItems.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase {
        private readonly LocationService _locationService;
        private readonly IMapper _mapper;

        public LocationController(LocationService locationService, IMapper mapper) {
            _locationService = locationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations() {
            List<Location> locations = await _locationService.GetAllLocations();

            if (locations == null) {
                return NotFound();
            }

            return Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationById(int id) {
            Location? location = await _locationService.GetLocationById(id);

            if ((location == null)) {
                return NotFound();
            }

            return Ok(location);
        }

        [HttpPost]
        public async Task<IActionResult> AddLocation(LocationDTO locationDTO) {
            var (result, createdLocation) = await _locationService.AddLocation(locationDTO);

            if (!result.Success) {
                return BadRequest(new { error = result.Message });
            }

            LocationDTO createdLocationDTO = _mapper.Map<LocationDTO>(createdLocation);

            return CreatedAtAction(nameof(GetLocationById), new { id = createdLocation.LocationId }, createdLocationDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, Location location) {
            if (id != location.LocationId) {
                return BadRequest();
            }

            ServiceResult result = await _locationService.UpdateLocation(location);

            if (!result.Success) {
                if (result.Code == Enums.Status.NotFound) {
                    return NotFound(new { error = (result.Message) });
                }

                return BadRequest(new { error = (result.Message) });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id) {
            Location? location = await _locationService.GetLocationById(id);

            if (location == null) {
                return NotFound();
            }

            await _locationService.DeleteLocation(location);
            return NoContent();
        }
    }
}
