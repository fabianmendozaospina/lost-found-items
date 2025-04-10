using AutoMapper;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.DAL;
using LostAndFoundItems.Models;

namespace LostAndFoundItems.BLL
{
    public class CategoryService {
        public readonly CategoryRepository _categoryRepository;
        public readonly IMapper _mapper;

        public CategoryService(CategoryRepository categoryRepository, IMapper mapper) 
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryDTO>> GetAllCategories() 
        {
            List<Category> categories = await _categoryRepository.GetAllCategories();

            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO?> GetCategoryById(int id) 
        {
            Category category = await _categoryRepository.GetCategoryById(id);

            if (category == null)
            {
                return null;
            }

            CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(category);

            return categoryDTO;
        }

        public async Task<(ServiceResult Result, CategoryDTO? CreatedCategory)> AddCategory(CategoryWriteDTO categoryDTO) 
        {
            try 
            {
                Category category = _mapper.Map<Category>(categoryDTO);
                Category createdCategory = await _categoryRepository.AddCategory(category);
                CategoryDTO createdCategoryDTO = _mapper.Map<CategoryDTO>(createdCategory);

                return (ServiceResult.Ok(), createdCategoryDTO);
            }
            catch (Exception ex) 
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return (ServiceResult.Fail($"Error: {message}"), null);
            }
        }

        public async Task<ServiceResult> UpdateCategory(int id, CategoryWriteDTO categoryDTO) 
        {
            try 
            {
                Category category = _mapper.Map<Category>(categoryDTO);
                category.CategoryId = id;

                await _categoryRepository.UpdateCategory(category);

                return ServiceResult.Ok();
            }
            catch (Exception ex) 
            {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR)) 
                {
                    return ServiceResult.Fail($"Category {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }

        public async Task<ServiceResult> DeleteCategory(int id) 
        {
            try 
            {
                Category category = await _categoryRepository.GetCategoryById(id);

                if (category == null)
                {
                    return ServiceResult.Fail($"Category {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                await _categoryRepository.DeleteCategory(category);

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
