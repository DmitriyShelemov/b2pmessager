using FluentValidation;
using facadeservice.Dto;

namespace facadeservice.Validators
{
    public class MessageUpdateDtoValidator : AbstractValidator<MessageUpdateDto>
    {
        public MessageUpdateDtoValidator()
        {
            RuleFor(x => x.MessageText)
                .MinimumLength(1)
                .MaximumLength(20000);
        }
    }
}
