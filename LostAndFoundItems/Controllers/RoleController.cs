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

        /// <summary>
        /// Gets all existing roles.
        /// </summary>
        /// <returns>A list of roles or 404 if none exist.</returns>
        /// <response code="200">Returns the list of roles.</response>
        /// <response code="404">No roles found.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<RoleDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoles()
        {
            List<RoleDTO> roleDTOList = await _roleService.GetRoles();

            if (roleDTOList == null || roleDTOList.Count == 0)
            {
                return NotFound(new ErrorResponseDTO { Error = Constants.NOT_ROLES_FOUND });
            }

            return Ok(roleDTOList);
        }

        /// <summary>
        /// Gets a specific role by its ID.
        /// </summary>
        /// <param name="id">The role ID.</param>
        /// <returns>The requested role, or 404 if not found.</returns>
        /// <response code="200">Role found.</response>
        /// <response code="404">Role not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoleDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoleById(int id)
        {
            RoleDTO roleDTO = await _roleService.GetRoleById(id);

            if (roleDTO == null)
            {
                return NotFound(new ErrorResponseDTO { Error = $"Role {Constants.NOT_FOUND}" });
            }

            return Ok(roleDTO);
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="roleDTO">The role data to create (only name is required).</param>
        /// <returns>The created role with its generated ID.</returns>
        /// <response code="201">Role successfully created.</response>
        /// <response code="400">Invalid input or business logic error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(RoleDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRole(RoleWriteDTO roleDTO)
        {
            (ServiceResult result, RoleDTO createdRoleDTO) = await _roleService.AddRole(roleDTO);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return CreatedAtAction(nameof(GetRoleById), new { id = createdRoleDTO.RoleId }, createdRoleDTO);
        }

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        /// <param name="id">The ID of the role to update.</param>
        /// <param name="roleDTO">The new role data.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">Role updated successfully.</response>
        /// <response code="400">Invalid input or operation failed.</response>
        /// <response code="404">Role not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRole(int id, RoleWriteDTO roleDTO)
        {
            ServiceResult result = await _roleService.UpdateRole(id, roleDTO);

            if (!result.Success)
            {
                if (result.Code == Enums.Status.NotFound)
                {
                    return NotFound(new ErrorResponseDTO { Error = result.Message });
                }

                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes an existing role by ID.
        /// </summary>
        /// <param name="id">The ID of the role to delete.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">Role deleted successfully.</response>
        /// <response code="400">Operation failed.</response>
        /// <response code="404">Role not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            ServiceResult result = await _roleService.DeleteRole(id);

            if (!result.Success)
            {
                if (result.Code == Enums.Status.NotFound)
                {
                    return NotFound(new ErrorResponseDTO { Error = result.Message });
                }

                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return NoContent();
        }
    }
}