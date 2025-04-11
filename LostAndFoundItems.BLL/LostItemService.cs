using AutoMapper;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.Models;
using LostAndFoundItems.DAL;

namespace LostAndFoundItems.BLL
{
    public class LostItemService
    {
        private readonly LostItemRepository _LostItemRepository;
        private readonly IMapper _mapper;

        public LostItemService(LostItemRepository LostItemRepository, IMapper mapper)
        {
            _LostItemRepository = LostItemRepository;
            _mapper = mapper;
        }

        public async Task<List<LostItemDTO>> GetAllLostItems()
        {
            List<LostItem> LostItems = await _LostItemRepository.GetAllLostItems();
            return LostItems.Select(MapLostItemToDTO).ToList();
        }

        public async Task<LostItemDTO?> GetLostItemById(int id)
        {
            LostItem LostItem = await _LostItemRepository.GetLostItemById(id);

            if (LostItem == null)
            {
                return null;
            }

            return MapLostItemToDTO(LostItem);
        }

        public async Task<(ServiceResult Result, LostItemDTO? CreatedLostItemDTO)> AddLostItem(LostItemWriteDTO LostItemDTO)
        {
            try
            {
                LostItem LostItem = _mapper.Map<LostItem>(LostItemDTO);
                LostItem createdLostItem = await _LostItemRepository.AddLostItem(LostItem);
                LostItemDTO createdLostItemDTO = new LostItemDTO
                {
                    LostItemId = createdLostItem.LostItemId,
                    UserId = createdLostItem.UserId,
                    UserFullName = $"{createdLostItem.User.FirstName} {createdLostItem.User.LastName}",
                    LocationId = createdLostItem.LocationId,
                    LocationName = createdLostItem.Location.Name,
                    CategoryId = createdLostItem.CategoryId,
                    CategoryName = createdLostItem.Category.Name,
                    Title = createdLostItem.Title,
                    Description = createdLostItem.Description,
                    LostDate = createdLostItem.LostDate
                };

                return (ServiceResult.Ok(), createdLostItemDTO);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return (ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}"), null);
            }
        }

        public async Task<ServiceResult> UpdateLostItem(int id, LostItemWriteDTO LostItemDTO)
        {
            try
            {
                LostItem LostItem = _mapper.Map<LostItem>(LostItemDTO);
                LostItem.LostItemId = id;

                await _LostItemRepository.UpdateLostItem(LostItem);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR))
                {
                    return ServiceResult.Fail($"Lost item {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }

        public async Task<ServiceResult> DeleteLostItem(int id)
        {
            try
            {
                LostItem LostItem = await _LostItemRepository.GetLostItemById(id);

                if (LostItem == null)
                {
                    return ServiceResult.Fail($"Lost item {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                await _LostItemRepository.DeleteLostItem(LostItem);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }
        private LostItemDTO MapLostItemToDTO(LostItem fi)
        {
            return new LostItemDTO
            {
                LostItemId = fi.LostItemId,
                UserId = fi.UserId,
                UserFullName = fi.User != null ? $"{fi.User.FirstName} {fi.User.LastName}" : "Unknown",
                LocationId = fi.LocationId,
                LocationName = fi.Location?.Name ?? "Unknown",
                CategoryId = fi.CategoryId,
                CategoryName = fi.Category?.Name ?? "Unknown",
                Title = fi.Title,
                Description = fi.Description,
                LostDate = fi.LostDate
            };
        }
    }
}