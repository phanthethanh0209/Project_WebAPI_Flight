using FluentValidation;
using TheThanh_WebAPI_Flight.DTO;

namespace TheThanh_WebAPI_RobotHeineken.Validation
{
    public class RoleValidator : AbstractValidator<RoleDTO>
    {
        public RoleValidator()
        {
            RuleFor(role => role.Name)
               .NotEmpty().WithMessage("Name role is required.");

            RuleFor(role => role.Note)
                .NotEmpty().WithMessage("Note is required.");
        }
    }
}
