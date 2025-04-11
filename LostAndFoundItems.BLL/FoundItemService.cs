using AutoMapper;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.DAL;
using LostAndFoundItems.Models;
using Microsoft.EntityFrameworkCore;

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
            List<FoundItemDTO> foundItemsDTO = foundItems
                                                .Select(fi => new FoundItemDTO
                                                {
                                                    FoundItemId = fi.FoundItemId,
                                                    UserId = fi.UserId,
                                                    UserFullName = fi.User.FirstName + " " + fi.User.LastName,
                                                    LocationId = fi.LocationId,
                                                    LocationName = fi.Location.Name,
                                                    CategoryId = fi.CategoryId,
                                                    CategoryName = fi.Category.Name,
                                                    Title = fi.Title,
                                                    Description = fi.Description,
                                                    FoundDate = fi.FoundDate
                                                }).ToList();

            return foundItemsDTO;
        }

        public async Task<FoundItemDTO?> GetFoundItemById(int id)
        {
            FoundItem foundItem = await _foundItemRepository.GetFoundItemById(id);

            if (foundItem == null)
            {
                return null;
            }

            FoundItemDTO foundItemDTO = new FoundItemDTO()
            {
                FoundItemId = foundItem.FoundItemId,
                UserId = foundItem.UserId,
                UserFullName = foundItem.User.FirstName + " " + foundItem.User.LastName,
                LocationId = foundItem.LocationId,
                LocationName = foundItem.Location.Name,
                CategoryId = foundItem.CategoryId,
                CategoryName = foundItem.Category.Name,
                Title = foundItem.Title,
                Description = foundItem.Description,
                FoundDate = foundItem.FoundDate
            };

            return foundItemDTO;
        }

        public async Task<(ServiceResult Result, FoundItemDTO? CreatedFoundItemDTO)> AddFoundItem(FoundItemWriteDTO foundItemDTO)
        {
            try
            {
                FoundItem foundItem = _mapper.Map<FoundItem>(foundItemDTO);
                FoundItem createdFoundItem = await _foundItemRepository.AddFoundItem(foundItem);

                // Load navegation data from database.
                //User user = await _context.Users.FindAsync(createdFoundItem.UserId);
                //Location location = await _context.Locations.FindAsync(createdFoundItem.LocationId);
                //Category category = await _context.Categories.FindAsync(createdFoundItem.CategoryId);

                FoundItemDTO createdFoundItemDTO = new FoundItemDTO
                {
                    FoundItemId = createdFoundItem.FoundItemId,
                    UserId = createdFoundItem.UserId,
                    UserFullName = $"{createdFoundItem.User.FirstName} {createdFoundItem.User.LastName}", //$"{user.FirstName} {user.LastName}",
                    LocationId = createdFoundItem.LocationId,
                    LocationName = createdFoundItem.Location.Name, //location?.Name,
                    CategoryId = createdFoundItem.CategoryId,
                    CategoryName = createdFoundItem.Category.Name, //category?.Name,
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
                    return ServiceResult.Fail($"FoundItem {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
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
                    return ServiceResult.Fail($"FoundItem {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
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
    }
}