using AutoMapper;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Repository;
using TheThanh_WebAPI_Flight.Validation;

namespace TheThanh_WebAPI_Flight.Services
{
    public interface IRolePermissionService
    {
        Task<IEnumerable<RolePermissionDTO>> GetAllRolePermission();
        Task<IEnumerable<RolePermissionDTO>> GetPerrmissionInRole(int id);
        Task<(bool Success, string ErrorMessage)> AddPermissionToRole(RolePermissionDTO createDTO);
        Task<(bool Success, string ErrorMessage)> DeleteRolePermission(int roleId, int permissionId);

    }
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly RolePermissionValidator _rolePermissionValidator;

        public RolePermissionService(IRepositoryWrapper repository, IMapper mapper, RolePermissionValidator rolePermissionValidator)
        {
            _repository = repository;
            _mapper = mapper;
            _rolePermissionValidator = rolePermissionValidator;
        }

        public async Task<(bool Success, string ErrorMessage)> AddPermissionToRole(RolePermissionDTO createDTO)
        {
            FluentValidation.Results.ValidationResult validationResult = await _rolePermissionValidator.ValidateAsync(createDTO);
            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            RolePermission newRolePermission = _mapper.Map<RolePermission>(createDTO);
            await _repository.RolePermission.CreateAsync(newRolePermission);

            return (true, null);
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteRolePermission(int roleId, int perrmissionId)
        {
            RolePermission role = await _repository.RolePermission.GetByIdAsync(m => m.RoleID == roleId && m.PermissionID == perrmissionId);

            if (role == null) return (false, "RolePermission not found");

            await _repository.RolePermission.DeleteAsync(role);
            return (true, null);
        }

        public async Task<IEnumerable<RolePermissionDTO>> GetAllRolePermission()
        {
            IEnumerable<RolePermission> roles = await _repository.RolePermission.GetAllAsync();
            return _mapper.Map<IEnumerable<RolePermissionDTO>>(roles);
        }

        public async Task<IEnumerable<RolePermissionDTO>> GetPerrmissionInRole(int id)
        {
            IEnumerable<RolePermission> roleUsers = await _repository.RolePermission.GetAllAsync(m => m.RoleID == id);
            if (!roleUsers.Any())
            {
                return null;
            }
            return _mapper.Map<IEnumerable<RolePermissionDTO>>(roleUsers);
        }
    }
}
