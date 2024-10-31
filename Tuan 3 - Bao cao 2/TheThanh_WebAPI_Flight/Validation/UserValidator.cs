using FluentValidation;
using TheThanh_WebAPI_Flight.DTO;

namespace TheThanh_WebAPI_Flight.Validation
{
    public class UserValidator : AbstractValidator<CreateUserDTO>
    {
        public UserValidator()
        {
            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Matches(@"^(?=.*[a-zA-Z])(?=.*\d).{8,}$").WithMessage("Username must be at least 8 characters long and contain both letters and numbers.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,30}$").WithMessage("Password must be between 8 and 30 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(user => user.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{10}$").WithMessage("Phone number must be exactly 10 digits and contain only numbers.");
        }
    }
}
