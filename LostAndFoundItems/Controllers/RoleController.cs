using AutoMapper;
using LostAndFoundItems.BLL;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.Models;
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
            List<Role> roles = await _roleService.GetRoles();

            if (roles == null)
            {
                return NotFound();
            }

            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            Role? role = await _roleService.GetRoleById(id);

            if ( (role == null))
            {
                return NotFound();
            }

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleDTO roleDTO)
        {
            var (result, createdRole) = await _roleService.AddRole(roleDTO);

            if (!result.Success)
            {
                return BadRequest(new { error = result.Message });
            }

            RoleDTO createdDto = _mapper.Map<RoleDTO>(createdRole);

            return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.RoleId }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, Role role)
        {
            if (id != role.RoleId)
            {
                return BadRequest();
            }

            ServiceResult result = await _roleService.UpdateRole(role);
            
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
            Role? role = await _roleService.GetRoleById(id);

            if (role == null)
            {
                return NotFound();
            }

            await _roleService.DeleteRole(role);
            return NoContent();
        }
    }
}
