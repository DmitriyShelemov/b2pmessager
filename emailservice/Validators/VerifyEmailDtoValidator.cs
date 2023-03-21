using FluentValidation;
using emailservice.Dto;

namespace emailservice.Validators
{
    public class VerifyEmailDtoValidator : AbstractValidator<VerifyEmailDto>
    {
        public VerifyEmailDtoValidator()
        {
            RuleFor(x => x.Link)
                .MinimumLength(1);

            RuleFor(x => x.To)
                .EmailAddress();
        }
    }
}
