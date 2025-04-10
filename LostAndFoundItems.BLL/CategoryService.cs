using AutoMapper;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.Common;
using LostAndFoundItems.DAL;
using LostAndFoundItems.Models;

namespace LostAndFoundItems.BLL {
    public class CategoryService {
        public readonly CategoryRepository _categoryRepository;
        public readonly IMapper _mapper;

        public CategoryService(CategoryRepository categoryRepository, IMapper mapper) {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<Category>> GetAllCategories() {
            return await _categoryRepository.GetAllCategories();
        }

        public async Task<Category> GetCategoryById(int id) {
            return await _categoryRepository.GetCategoryById(id);
        }

        public async Task<(ServiceResult Result, Category CreatedCategory)> AddCategory(CategoryDTO categoryDTO) {
            try {
                var category = _mapper.Map<Category>(categoryDTO);
                var createdCategory = await _categoryRepository.AddCategory(category);
                return (ServiceResult.Ok(), createdCategory);
            }
            catch (Exception ex) {
                return (ServiceResult.Fail($"Error: {ex.Message}"), null);
            }
        }

        public async Task<ServiceResult> UpdateCategory(Category category) {
            try {
                await _categoryRepository.UpdateCategory(category);
                return ServiceResult.Ok();
            }
            catch (Exception ex) {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR)) {
                    return ServiceResult.Fail($"Category {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{ex.Message}");
            }
        }

        public async Task<ServiceResult> DeleteCategory(Category category) {
            try {
                await _categoryRepository.DeleteCategory(category);
                return ServiceResult.Ok();
            }
            catch (Exception ex) {
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{ex.Message}");
            }
        }
    }
}
