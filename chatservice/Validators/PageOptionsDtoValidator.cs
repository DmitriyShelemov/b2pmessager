using FluentValidation;
using chatservice.Dto;

namespace chatservice.Validators
{
    public class PageOptionsDtoValidator : AbstractValidator<PageOptionsDto>
    {
        public PageOptionsDtoValidator()
        {
            RuleFor(x => x.Take)
                .LessThanOrEqualTo(PageOptionsDto.MaxTake);

            RuleFor(x => x.TenantUID)
                .Must(x => x != Guid.Empty);
        }
    }
}
