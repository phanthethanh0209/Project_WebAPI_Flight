using AutoMapper;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Repository;
using TheThanh_WebAPI_Flight.Validation;

namespace TheThanh_WebAPI_Flight.Services
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDTO>> GetAllPermission();
        Task<PermissionDTO> GetPermission(int id);
        Task<(bool Success, string ErrorMessage)> CreatePermission(PermissionDTO createDTO);
        Task<(bool Success, string ErrorMessage)> UpdatePermission(int id, PermissionDTO updateDTO);
        Task<(bool Success, string ErrorMessage)> DeletePermission(int id);

    }
    public class PermissionService : IPermissionService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly PermissionValidator _permissionValidator;

        public PermissionService(IRepositoryWrapper repository, IMapper mapper, PermissionValidator permissionValidator)
        {
            _repository = repository;
            _mapper = mapper;
            _permissionValidator = permissionValidator;
        }

        public async Task<(bool Success, string ErrorMessage)> CreatePermission(PermissionDTO createDTO)
        {
            FluentValidation.Results.ValidationResult validationResult = await _permissionValidator.ValidateAsync(createDTO);
            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            Permission newPermission = _mapper.Map<Permission>(createDTO);
            await _repository.Permission.CreateAsync(newPermission);

            return (true, null);
        }

        public async Task<(bool Success, string ErrorMessage)> DeletePermission(int id)
        {
            Permission permission = await _repository.Permission.GetByIdAsync(m => m.PermissionID == id);

            if (permission == null) return (false, "Permission not found");

            await _repository.Permission.DeleteAsync(permission);
            return (true, null);
        }

        public async Task<IEnumerable<PermissionDTO>> GetAllPermission()
        {
            IEnumerable<Permission> permissions = await _repository.Permission.GetAllAsync();
            return _mapper.Map<IEnumerable<PermissionDTO>>(permissions);
        }

        public async Task<PermissionDTO> GetPermission(int id)
        {
            Permission permission = await _repository.Permission.GetByIdAsync(m => m.PermissionID == id);

            if (permission == null) return null;

            return _mapper.Map<PermissionDTO>(permission);
        }

        public async Task<(bool Success, string ErrorMessage)> UpdatePermission(int id, PermissionDTO updateDTO)
        {
            Permission permission = await _repository.Permission.GetByIdAsync(m => m.PermissionID == id);

            FluentValidation.Results.ValidationResult validationResult = await _permissionValidator.ValidateAsync(updateDTO);

            if (!validationResult.IsValid)
                return (false, validationResult.Errors.First().ErrorMessage);

            _mapper.Map(updateDTO, permission);
            await _repository.Permission.UpdateAsync(permission);
            return (true, null);
        }
    }
}
