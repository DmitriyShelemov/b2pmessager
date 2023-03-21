using FluentValidation;
using userservice.Dto;

namespace userservice.Validators
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
