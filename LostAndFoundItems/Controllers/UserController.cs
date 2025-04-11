using AutoMapper;
using LostAndFoundItems.BLL;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFoundItems.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UserController(UserService userService, IMapper mapper) {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all existing users.
        /// </summary>
        /// <returns>A list of users or 404 if none exist.</returns>
        /// <response code="200">Returns the list of users.</response>
        /// <response code="404">No users found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers() {
            List<UserDTO> userDTOList = await _userService.GetAllUsers();

            if (userDTOList == null || userDTOList.Count == 0) {
                return NotFound(new ErrorResponseDTO { Error = Constants.NOT_USER_FOUND_ERROR });
            }

            return Ok(userDTOList);
        }

        /// <summary>
        /// Gets a specific user by its ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The requested user, or 404 if not found.</returns>
        /// <response code="200">User found.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(int id) {
            UserDTO userDTO = await _userService.GetUserById(id);

            if (userDTO == null) {
                return NotFound(new ErrorResponseDTO { Error = $"User {Constants.NOT_FOUND_ERROR}" });
            }

            return Ok(userDTO);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userDTO">The user data to create (only name is required).</param>
        /// <returns>The created user with its generated ID.</returns>
        /// <response code="201">User successfully created.</response>
        /// <response code="400">Invalid input or business logic error.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUser(UserWriteDTO userWriteDTO) {
            (ServiceResult result, UserDTO createdUserDTO) = await _userService.AddUser(userWriteDTO);

            if (!result.Success) {
                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return CreatedAtAction(nameof(GetUserById), new { id = createdUserDTO.UserId }, createdUserDTO);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userDTO">The new user data.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">User updated successfully.</response>
        /// <response code="400">Invalid input or operation failed.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(int id, UserWriteDTO userDTO) {
            ServiceResult result = await _userService.UpdateUser(id, userDTO);

            if (!result.Success) {
                if (result.Code == Enums.Status.NotFound) {
                    return NotFound(new ErrorResponseDTO { Error = result.Message });
                }

                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes an existing user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>204 No Content if successful, or appropriate error response.</returns>
        /// <response code="204">User deleted successfully.</response>
        /// <response code="400">Operation failed.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int id) {
            ServiceResult result = await _userService.DeleteUser(id);

            if (!result.Success) {
                if (result.Code == Enums.Status.NotFound) {
                    return NotFound(new ErrorResponseDTO { Error = result.Message });
                }

                return BadRequest(new ErrorResponseDTO { Error = result.Message });
            }

            return NoContent();
        }
    }
}