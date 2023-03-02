using FluentValidation;
using facadeservice.Dto;

namespace facadeservice.Validators
{
    public class MessageCreateDtoValidator : AbstractValidator<MessageCreateDto>
    {
        public MessageCreateDtoValidator()
        {
            RuleFor(x => x.MessageText)
                .MinimumLength(1)
                .MaximumLength(20000);
        }
    }
}
