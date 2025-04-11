using AutoMapper;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.Common;
using LostAndFoundItems.DAL;
using LostAndFoundItems.Models;

namespace LostAndFoundItems.BLL {
    public class UserService {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(UserRepository userRepository, IMapper mapper) {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<List<UserDTO>> GetAllUsers() {
            List<User> users = await _userRepository.GetAllUsers();

            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO?> GetUserById(int id) {
            User user = await _userRepository.GetUserById(id);

            if (user == null) {
                return null;
            }

            UserDTO userDTO = _mapper.Map<UserDTO>(user);

            return userDTO;
        }

        public async Task<(ServiceResult Result, UserDTO? CreatedUserDTO)> AddUser(UserWriteDTO userWriteDTO) {
            try {
                User user = _mapper.Map<User>(userWriteDTO);
                User createdUser = await _userRepository.AddUser(user);
                UserDTO createdUserDTO = _mapper.Map<UserDTO>(createdUser);

                return (ServiceResult.Ok(), createdUserDTO);
            }
            catch (Exception ex) {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return (ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}"), null);
            }
        }

        public async Task<ServiceResult> UpdateUser(int id, UserWriteDTO userDTO) {
            try {
                User user = _mapper.Map<User>(userDTO);
                user.UserId = id;

                await _userRepository.UpdateUser(user);

                return ServiceResult.Ok();
            }
            catch (Exception ex) {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR)) {
                    return ServiceResult.Fail($"User {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }

        public async Task<ServiceResult> DeleteUser(int id) {
            try {
                User user = await _userRepository.GetUserById(id);

                if (user == null) {
                    return ServiceResult.Fail($"User {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                await _userRepository.DeleteUser(user);

                return ServiceResult.Ok();
            }
            catch (Exception ex) {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }

    }
}
