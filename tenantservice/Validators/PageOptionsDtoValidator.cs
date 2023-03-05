using FluentValidation;
using tenantservice.Dto;

namespace tenantservice.Validators
{
    public class PageOptionsDtoValidator : AbstractValidator<PageOptionsDto>
    {
        public PageOptionsDtoValidator()
        {
            RuleFor(x => x.Take)
                .LessThanOrEqualTo(PageOptionsDto.MaxTake);
        }
    }
}
