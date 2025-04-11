using AutoMapper;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.Models;
using LostAndFoundItems.DAL;

namespace LostAndFoundItems.BLL
{
    public class MatchItemService
    {
        private readonly MatchItemRepository _MatchItemRepository;
        private readonly IMapper _mapper;

        public MatchItemService(MatchItemRepository MatchItemRepository, IMapper mapper)
        {
            _MatchItemRepository = MatchItemRepository;
            _mapper = mapper;
        }

        public async Task<List<MatchItemDTO>> GetAllMatchItems()
        {
            List<MatchItem> MatchItems = await _MatchItemRepository.GetAllMatchItems();
            return MatchItems.Select(MapMatchItemToDTO).ToList();
        }

        public async Task<MatchItemDTO?> GetMatchItemById(int id)
        {
            MatchItem MatchItem = await _MatchItemRepository.GetMatchItemById(id);

            if (MatchItem == null)
            {
                return null;
            }

            return MapMatchItemToDTO(MatchItem);
        }

        public async Task<(ServiceResult Result, MatchItemDTO? CreatedMatchItemDTO)> AddMatchItem(MatchItemWriteDTO MatchItemDTO)
        {
            try
            {
                MatchItem MatchItem = _mapper.Map<MatchItem>(MatchItemDTO);
                MatchItem createdMatchItem = await _MatchItemRepository.AddMatchItem(MatchItem);
                MatchItemDTO createdMatchItemDTO = new MatchItemDTO
                {
                    MatchItemId = createdMatchItem.MatchItemId,
                    FoundItemId = createdMatchItem.FoundItemId,
                    FoundItemTitle = createdMatchItem.FoundItem.Title,
                    LostItemId = createdMatchItem.LostItemId,
                    LostItemIdTitle = createdMatchItem.LostItem.Title,
                    MatchUserId = createdMatchItem.MatchUserId,
                    MatchUserName = $"{createdMatchItem.User.FirstName} {createdMatchItem.User.LastName}",
                    MatchStatusId = createdMatchItem.MatchStatusId,
                    MatchStatusName = createdMatchItem.MatchStatus.Name,
                    Observation = createdMatchItem.Observation
                };

                return (ServiceResult.Ok(), createdMatchItemDTO);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return (ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}"), null);
            }
        }

        public async Task<ServiceResult> UpdateMatchItem(int id, MatchItemWriteDTO MatchItemDTO)
        {
            try
            {
                MatchItem MatchItem = _mapper.Map<MatchItem>(MatchItemDTO);
                MatchItem.MatchItemId = id;

                await _MatchItemRepository.UpdateMatchItem(MatchItem);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR))
                {
                    return ServiceResult.Fail($"Match item {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }

        public async Task<ServiceResult> DeleteMatchItem(int id)
        {
            try
            {
                MatchItem MatchItem = await _MatchItemRepository.GetMatchItemById(id);

                if (MatchItem == null)
                {
                    return ServiceResult.Fail($"Match item {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                await _MatchItemRepository.DeleteMatchItem(MatchItem);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }
        private MatchItemDTO MapMatchItemToDTO(MatchItem fi)
        {
            return new MatchItemDTO
            {
                MatchItemId = fi.MatchItemId,
                FoundItemId = fi.FoundItemId,
                FoundItemTitle = fi.FoundItem.Title ?? "Unknown",
                LostItemId = fi.LostItemId,
                LostItemIdTitle = fi.LostItem.Title ?? "Unknown",
                MatchUserId = fi.MatchUserId,
                MatchUserName = fi.User != null ? $"{fi.User.FirstName} {fi.User.LastName}" : "Unknown",
                MatchStatusId = fi.MatchStatusId,
                MatchStatusName = fi.MatchStatus?.Name ?? "Unknown",
                Observation = fi.Observation
            };
        }
    }
}