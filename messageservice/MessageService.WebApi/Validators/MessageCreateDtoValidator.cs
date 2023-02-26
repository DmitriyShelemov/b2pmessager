using FluentValidation;
using MessageService.WebApi.Dto;

namespace MessageService.WebApi.Validators
{
    public class MessageCreateDtoValidator : AbstractValidator<MessageCreateDto>
    {
        public MessageCreateDtoValidator()
        {
            RuleFor(x => x.MessageText)
                .MinimumLength(1)
                .WithMessage("The 'MessageText' should have at least 1 character.")
                .MaximumLength(20000)
                .WithMessage("The 'MessageText' should have not more than 100 characters.");
        }
    }
}
