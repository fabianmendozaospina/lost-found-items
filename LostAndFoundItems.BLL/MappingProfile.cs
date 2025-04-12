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

            CreateMap<ClaimStatusDTO, ClaimStatus>();
            CreateMap<ClaimStatus, ClaimStatusDTO>();
            CreateMap<ClaimStatusWriteDTO, ClaimStatus>();

            CreateMap<MatchStatusDTO, MatchStatus>();
            CreateMap<MatchStatus, MatchStatusDTO>();
            CreateMap<MatchStatusWriteDTO, MatchStatus>();

            CreateMap<FoundItemDTO, FoundItem>();
            CreateMap<FoundItemWriteDTO, FoundItem>()
                .ForMember(dest => dest.ClaimRequests, opt => opt.Ignore())
                .ForMember(dest => dest.MatchItem, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore());

            CreateMap<LostItemDTO, LostItem>();
            CreateMap<LostItemWriteDTO, LostItem>()
                .ForMember(dest => dest.MatchItem, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore());

            CreateMap<MatchItemDTO, MatchItem>();
            CreateMap<MatchItemWriteDTO, MatchItem>()
                .ForMember(dest => dest.FoundItem, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.LostItem, opt => opt.Ignore())
                .ForMember(dest => dest.MatchStatus, opt => opt.Ignore());

            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));
            CreateMap<UserWriteDTO, User>();

            CreateMap<ClaimRequestDTO, ClaimRequest>();
            CreateMap<ClaimRequestWriteDTO, ClaimRequest>()
                .ForMember(dest => dest.ClaimStatus, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.FoundItem, opt => opt.Ignore());
        }
    }
}
