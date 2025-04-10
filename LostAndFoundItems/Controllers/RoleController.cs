using AutoMapper;
using LostAndFoundItems.BLL;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(RoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            List<RoleDTO> roleDTOList = await _roleService.GetRoles();

            if (roleDTOList == null || roleDTOList.Count == 0)
            {
                return NotFound();
            }

            return Ok(roleDTOList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            RoleDTO roleDTO = await _roleService.GetRoleById(id);

            if (roleDTO == null)
            {
                return NotFound();
            }

            return Ok(roleDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleWriteDTO roleDTO)
        {
            (ServiceResult result, RoleDTO createdRoleDTO) = await _roleService.AddRole(roleDTO);

            if (!result.Success)
            {
                return BadRequest(new { error = result.Message });
            }

            return CreatedAtAction(nameof(GetRoleById), new { id = createdRoleDTO.RoleId }, createdRoleDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, RoleWriteDTO roleDTO)
        {
            ServiceResult result = await _roleService.UpdateRole(id, roleDTO);
            
            if (!result.Success)
            {
                if (result.Code == Enums.Status.NotFound)
                {
                    return NotFound(new { error = (result.Message) });
                }
                
                return BadRequest(new { error = (result.Message) });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            ServiceResult result = await _roleService.DeleteRole(id);

            if (!result.Success)
            {
                if (result.Code == Enums.Status.NotFound)
                {
                    return NotFound(new { error = result.Message });
                }

                return BadRequest(new { error = result.Message });
            }

            return NoContent();
        }
    }
}
