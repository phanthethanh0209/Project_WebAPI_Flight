using FluentValidation;
using TheThanh_WebAPI_Flight.DTO;

namespace TheThanh_WebAPI_Flight.Validation
{
    public class RolePermissionValidator : AbstractValidator<RolePermissionDTO>
    {
        public RolePermissionValidator()
        {
            RuleFor(role => role.RoleID)
               .NotEmpty().WithMessage("Role ID is required.");

            RuleFor(role => role.PermissionID)
                .NotEmpty().WithMessage("Permission ID is required.");
        }

    }
}
