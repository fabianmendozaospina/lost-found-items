using AutoMapper;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.DAL;
using LostAndFoundItems.Models;

namespace LostAndFoundItems.BLL
{
    public class LocationService
    {
        private readonly LocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public LocationService(LocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<List<LocationDTO>> GetAllLocations()
        {
            List<Location> locations = await _locationRepository.GetAllLocations();

            return _mapper.Map<List<LocationDTO>>(locations);
        }

        public async Task<LocationDTO?> GetLocationById(int id)
        {
            Location location = await _locationRepository.GetLocationById(id);

            if (location == null)
            {
                return null;
            }

            LocationDTO locationDTO = _mapper.Map<LocationDTO>(location);

            return locationDTO;
        }

        public async Task<(ServiceResult Result, LocationDTO? CreatedLocationDTO)> AddLocation(LocationWriteDTO locationDTO)
        {
            try
            {
                Location location = _mapper.Map<Location>(locationDTO);
                Location createdLocation = await _locationRepository.AddLocation(location);
                LocationDTO createdLocationDTO = _mapper.Map<LocationDTO>(createdLocation);

                return (ServiceResult.Ok(), createdLocationDTO);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return (ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}"), null);
            }
        }

        public async Task<ServiceResult> UpdateLocation(int id, LocationWriteDTO locationDTO)
        {
            try
            {
                Location location = _mapper.Map<Location>(locationDTO);
                location.LocationId = id;

                await _locationRepository.UpdateLocation(location);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR))
                {
                    return ServiceResult.Fail($"Location {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }

        public async Task<ServiceResult> DeleteLocation(int id)
        {
            try
            {
                Location location = await _locationRepository.GetLocationById(id);

                if (location == null)
                {
                    return ServiceResult.Fail($"Location {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                await _locationRepository.DeleteLocation(location);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }
    }
}
