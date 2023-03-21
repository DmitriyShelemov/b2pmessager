using FluentValidation;
using facadeservice.Dto;

namespace facadeservice.Validators
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .Null();

            RuleFor(x => x.FirstName)
                .MinimumLength(1)
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .MinimumLength(1)
                .MaximumLength(100);

            RuleFor(x => x.Password)
                .Null();

            RuleFor(x => x.Email)
                .Null();
        }
    }
}
