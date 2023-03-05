using FluentValidation;
using tenantservice.Dto;

namespace tenantservice.Validators
{
    public class GuidDtoValidator : AbstractValidator<GuidDto>
    {
        public GuidDtoValidator()
        {
            RuleFor(x => x.Id)
                .Must(x => x != Guid.Empty);
        }
    }
}
