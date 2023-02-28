using FluentValidation;
using messageservice.Dto;

namespace messageservice.Validators
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
