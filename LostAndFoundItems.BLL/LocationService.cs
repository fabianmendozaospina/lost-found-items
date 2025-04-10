using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.DAL;
using LostAndFoundItems.Models;

namespace LostAndFoundItems.BLL {
    public  class LocationService {
        public readonly LocationRepository _locationRepository;
        public readonly IMapper _mapper;

        public LocationService(LocationRepository locationRepository, IMapper mapper) {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<List<Location>> GetAllLocations() {
            return await _locationRepository.GetAllLocations();
        }

        public async Task<Location> GetLocationById(int id) {
            return await _locationRepository.GetLocationById(id);
        }

        public async Task<(ServiceResult Result, Location CreatedLocation)> AddLocation(LocationDTO locationDTO) {
            try {
                var location = _mapper.Map<Location>(locationDTO);
                var createdLocation = await _locationRepository.AddLocation(location);
                return (ServiceResult.Ok(), createdLocation);
            }
            catch (Exception ex) {
                return (ServiceResult.Fail($"Error: {ex.Message}"), null);
            }
        }

        public async Task<ServiceResult> UpdateLocation(Location location) {
            try {
                await _locationRepository.UpdateLocation(location);
                return ServiceResult.Ok();
            }
            catch (Exception ex) {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR)) {
                    return ServiceResult.Fail($"Location {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{ex.Message}");
            }
        }

        public async Task<ServiceResult> DeleteLocation(Location location) {
            try {
                await _locationRepository.DeleteLocation(location);
                return ServiceResult.Ok();
            }
            catch (Exception ex) {
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{ex.Message}");
            }
        }
    }
}
