using FluentValidation;
using messageservice.Dto;

namespace messageservice.Validators
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
