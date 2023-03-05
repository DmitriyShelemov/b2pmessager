using FluentValidation;
using chatservice.Dto;

namespace chatservice.Validators
{
    public class GuidDtoValidator : AbstractValidator<GuidDto>
    {
        public GuidDtoValidator()
        {
            RuleFor(x => x.Id)
                .Must(x => x != Guid.Empty);

            RuleFor(x => x.TenantUID)
                .Must(x => x != Guid.Empty);
        }
    }
}
