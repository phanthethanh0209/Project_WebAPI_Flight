using FluentValidation;
using TheThanh_WebAPI_Flight.DTO;

namespace TheThanh_WebAPI_Flight.Validation
{
    public class PermissionValidator : AbstractValidator<PermissionDTO>
    {
        public PermissionValidator()
        {
            RuleFor(role => role.Name)
               .NotEmpty().WithMessage("Name permission is required.");
        }
    }
}
