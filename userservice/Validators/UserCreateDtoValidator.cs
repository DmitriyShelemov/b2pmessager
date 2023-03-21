using FluentValidation;
using userservice.Dto;

namespace userservice.Validators
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

            RuleFor(x => x.Password)
                .MinimumLength(1)
                .MaximumLength(100)
                .When(x => x.EventType == CrudActionType.Signup);

            RuleFor(x => x.Password)
                .Null()
                .When(x => x.EventType != CrudActionType.Signup);

            RuleFor(x => x.Email)
                .EmailAddress();
        }
    }
}
