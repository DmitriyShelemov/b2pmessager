using FluentValidation;
using tenantservice.Dto;

namespace tenantservice.Validators
{
    public class TenantCreateDtoValidator : AbstractValidator<TenantCreateDto>
    {
        public TenantCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(1)
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .EmailAddress();
        }
    }
}
