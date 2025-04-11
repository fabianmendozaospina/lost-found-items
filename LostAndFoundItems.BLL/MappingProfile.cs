using AutoMapper;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.Models;

namespace LostAndFoundItems.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryWriteDTO, Category>();

            CreateMap<LocationDTO, Location>();
            CreateMap<Location, LocationDTO>();
            CreateMap<LocationWriteDTO, Location>();

            CreateMap<RoleDTO, Role>();
            CreateMap<Role, RoleDTO>();
            CreateMap<RoleWriteDTO, Role>();

            CreateMap<FoundItem, FoundItemDTO>();
        }
    }
}
