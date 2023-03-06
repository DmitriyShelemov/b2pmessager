using FluentValidation;
using facadeservice.Dto;

namespace facadeservice.Validators
{
    public class TenantCreateDtoValidator : AbstractValidator<TenantCreateDto>
    {
        public TenantCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(1)
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .MinimumLength(1)
                .EmailAddress();
        }
    }
}
