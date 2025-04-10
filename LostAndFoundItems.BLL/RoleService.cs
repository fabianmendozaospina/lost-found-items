using AutoMapper;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.DAL;
using LostAndFoundItems.Models;

namespace LostAndFoundItems.BLL
{
    public class RoleService
    {
        private readonly RoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(RoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<List<Role>> GetRoles()
        {
            return await _roleRepository.GetAllRoles();
        }

        public async Task<Role> GetRoleById(int id)
        {
            return await _roleRepository.GetRoleById(id);
        }

        public async Task<(ServiceResult Result, Role CreatedRole)> AddRole(RoleDTO roleDTO)
        {
            try
            {
                var role = _mapper.Map<Role>(roleDTO);
                var createdRole = await _roleRepository.AddRole(role);

                return (ServiceResult.Ok(), createdRole);
            }
            catch (Exception ex)
            {
                return (ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{ex.Message}"), null);
            }
        }

        public async Task<ServiceResult> UpdateRole(Role role)
        {
            try
            {
                await _roleRepository.UpdateRole(role);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR))
                {
                    return ServiceResult.Fail($"Role {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{ex.Message}");
            }
        }

        public async Task<ServiceResult> DeleteRole(Role role)
        {
            try
            {
                await _roleRepository.DeleteRole(role);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{ex.Message}");
            }
        }
    }
}
