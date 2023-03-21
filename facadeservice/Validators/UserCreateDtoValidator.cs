using FluentValidation;
using facadeservice.Dto;

namespace facadeservice.Validators
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(1)
                .MaximumLength(100);

            RuleFor(x => x.FirstName)
                .MinimumLength(1)
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .MinimumLength(1)
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .EmailAddress();
        }
    }
}
