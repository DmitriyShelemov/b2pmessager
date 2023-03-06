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

            RuleFor(x => x.ChatUID)
                .Must(x => x != Guid.Empty)
                .When(x => x.MessageUID == Guid.Empty);

            RuleFor(x => x.TenantUID)
                .Must(x => x != Guid.Empty);
        }
    }
}
