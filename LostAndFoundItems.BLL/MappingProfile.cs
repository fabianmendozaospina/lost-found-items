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
        }
    }
}
