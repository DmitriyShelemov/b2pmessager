using FluentValidation;
using userservice.Dto;

namespace userservice.Validators
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
