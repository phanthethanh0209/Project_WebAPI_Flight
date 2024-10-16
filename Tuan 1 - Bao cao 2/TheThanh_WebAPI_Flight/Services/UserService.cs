using AutoMapper;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Repository;
using TheThanh_WebAPI_Flight.Validation;

namespace TheThanh_WebAPI_Flight.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUser();
        Task<UserDTO> GetUserAsync(string name);
        Task<(bool Success, string ErrorMessage)> CreateUser(CreateUserDTO createDTO);
        Task<(bool Success, string ErrorMessage)> UpdateUser(int id, UserDTO updateLocationDTO);
        Task<(bool Success, string ErrorMessage)> DeleteUser(int id);
    }
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly UserValidator _createValidator;
        public UserService(IRepositoryWrapper repository, IMapper mapper, UserValidator createValidator)
        {
            _repository = repository;
            _mapper = mapper;
            _createValidator = createValidator;
        }

        public async Task<(bool Success, string ErrorMessage)> CreateUser(CreateUserDTO createDTO)
        {
            // Kiểm tra location name duy nhất
            bool isEmailUnique = await _repository.User.AnyAsync(m => m.Email == createDTO.Email);
            if (isEmailUnique)
            {
                return (false, "Email already exists");
            }


            FluentValidation.Results.ValidationResult validationResult = await _createValidator.ValidateAsync(createDTO);

            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            User newUser = _mapper.Map<User>(createDTO);

            // Hash the password before saving it to the database
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(createDTO.Password);

            await _repository.User.CreateAsync(newUser);
            return (true, null);
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteUser(int id)
        {
            User user = await _repository.User.GetByIdAsync(u => u.UserID == id);
            if (user == null)
                return (false, "User not found.");

            _repository.User.DeleteAsync(user);
            return (true, null);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUser()
        {
            IEnumerable<User> user = await _repository.User.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(user);

        }

        public async Task<UserDTO> GetUserAsync(string username)
        {
            User user = await _repository.User.GetByIdAsync(x => x.UserName == username);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserDTO>(user);
        }



        public async Task<(bool Success, string ErrorMessage)> UpdateUser(int id, UserDTO updateUserDTO)
        {
            User user = await _repository.User.GetByIdAsync(m => m.UserID == id);
            if (user == null)
            {
                return (false, "User not found");
            }

            _mapper.Map(updateUserDTO, user);
            await _repository.User.UpdateAsync(user);
            return (true, null);
        }
    }
}
