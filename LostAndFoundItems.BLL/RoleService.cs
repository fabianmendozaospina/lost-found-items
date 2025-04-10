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

        public async Task<List<RoleDTO>> GetRoles()
        {
            List<Role> roles = await _roleRepository.GetAllRoles();

            return _mapper.Map<List<RoleDTO>>(roles);
        }

        public async Task<RoleDTO> GetRoleById(int id)
        {
            Role role = await _roleRepository.GetRoleById(id);

            if (role == null)
            {
                return null;
            }

            RoleDTO roleDTO = _mapper.Map<RoleDTO>(role);

            return roleDTO;
        }

        public async Task<(ServiceResult Result, RoleDTO CreatedRoleDTO)> AddRole(RoleWriteDTO roleDTO)
        {
            try
            {
                Role role = _mapper.Map<Role>(roleDTO);
                Role createdRole = await _roleRepository.AddRole(role);
                RoleDTO createdRoleDTO = _mapper.Map<RoleDTO>(createdRole);

                return (ServiceResult.Ok(), createdRoleDTO);
            }
            catch (Exception ex)
            {
                return (ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{ex.Message}"), null);
            }
        }

        public async Task<ServiceResult> UpdateRole(int id, RoleWriteDTO roleDTO)
        {
            try
            {
                Role role = _mapper.Map<Role>(roleDTO);
                role.RoleId = id;

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

        public async Task<ServiceResult> DeleteRole(int id)
        {
            try
            {
                Role role = await _roleRepository.GetRoleById(id);

                if (role == null)
                {
                    return ServiceResult.Fail($"Role {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

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
