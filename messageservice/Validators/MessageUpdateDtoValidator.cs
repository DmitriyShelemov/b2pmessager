using FluentValidation;
using messageservice.Dto;

namespace messageservice.Validators
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
