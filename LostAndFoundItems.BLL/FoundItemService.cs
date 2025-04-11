using AutoMapper;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.DAL;
using LostAndFoundItems.Models;

namespace LostAndFoundItems.BLL
{
    public class FoundItemService
    {
        private readonly FoundItemRepository _foundItemRepository;
        private readonly IMapper _mapper;

        public FoundItemService(FoundItemRepository foundItemRepository, IMapper mapper)
        {
            _foundItemRepository = foundItemRepository;
            _mapper = mapper;
        }

        public async Task<List<FoundItemDTO>> GetAllFoundItems()
        {
            List<FoundItem> foundItems = await _foundItemRepository.GetAllFoundItems();
            return foundItems.Select(MapFoundItemToDTO).ToList();
        }

        public async Task<FoundItemDTO?> GetFoundItemById(int id)
        {
            FoundItem foundItem = await _foundItemRepository.GetFoundItemById(id);

            if (foundItem == null)
            {
                return null;
            }

            return MapFoundItemToDTO(foundItem);
        }

        public async Task<(ServiceResult Result, FoundItemDTO? CreatedFoundItemDTO)> AddFoundItem(FoundItemWriteDTO foundItemDTO)
        {
            try
            {
                FoundItem foundItem = _mapper.Map<FoundItem>(foundItemDTO);
                FoundItem createdFoundItem = await _foundItemRepository.AddFoundItem(foundItem);
                FoundItemDTO createdFoundItemDTO = new FoundItemDTO
                {
                    FoundItemId = createdFoundItem.FoundItemId,
                    UserId = createdFoundItem.UserId,
                    UserFullName = $"{createdFoundItem.User.FirstName} {createdFoundItem.User.LastName}",
                    LocationId = createdFoundItem.LocationId,
                    LocationName = createdFoundItem.Location.Name,
                    CategoryId = createdFoundItem.CategoryId,
                    CategoryName = createdFoundItem.Category.Name,
                    Title = createdFoundItem.Title,
                    Description = createdFoundItem.Description,
                    FoundDate = createdFoundItem.FoundDate
                };

                return (ServiceResult.Ok(), createdFoundItemDTO);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return (ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}"), null);
            }
        }

        public async Task<ServiceResult> UpdateFoundItem(int id, FoundItemWriteDTO foundItemDTO)
        {
            try
            {
                FoundItem foundItem = _mapper.Map<FoundItem>(foundItemDTO);
                foundItem.FoundItemId = id;

                await _foundItemRepository.UpdateFoundItem(foundItem);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR))
                {
                    return ServiceResult.Fail($"Found item {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }

        public async Task<ServiceResult> DeleteFoundItem(int id)
        {
            try
            {
                FoundItem foundItem = await _foundItemRepository.GetFoundItemById(id);

                if (foundItem == null)
                {
                    return ServiceResult.Fail($"Found item {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                await _foundItemRepository.DeleteFoundItem(foundItem);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }
        private FoundItemDTO MapFoundItemToDTO(FoundItem fi)
        {
            return new FoundItemDTO
            {
                FoundItemId = fi.FoundItemId,
                UserId = fi.UserId,
                UserFullName = fi.User != null ? $"{fi.User.FirstName} {fi.User.LastName}" : "Unknown",
                LocationId = fi.LocationId,
                LocationName = fi.Location?.Name ?? "Unknown",
                CategoryId = fi.CategoryId,
                CategoryName = fi.Category?.Name ?? "Unknown",
                Title = fi.Title,
                Description = fi.Description,
                FoundDate = fi.FoundDate,
                ClaimRequests = fi.ClaimRequests?.Select(cr => new ClaimRequestSimpleDTO
                {
                    ClaimRequestId = cr.ClaimRequestId,
                    ClaimingUserId = cr.ClaimingUserId,
                    ClaimingUserFullName = cr.User != null ? $"{cr.User.FirstName} {cr.User.LastName}" : "Unknown",
                    CreatedAt = cr.CreatedAt,
                    ClaimStatusId = cr.ClaimStatusId,
                    ClaimStatusName = cr.ClaimStatus?.Name ?? "Unknown",
                    Message = cr.Message
                }).ToList() ?? new List<ClaimRequestSimpleDTO>()
            };
        }
    }
}