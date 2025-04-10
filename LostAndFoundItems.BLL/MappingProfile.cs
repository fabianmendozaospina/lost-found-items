using AutoMapper;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.Models;

namespace LostAndFoundItems.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RoleDTO, Role>();
            CreateMap<Role, RoleDTO>();
            CreateMap<LocationDTO, Location>();
            CreateMap<Location, LocationDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();
        }
    }
}
