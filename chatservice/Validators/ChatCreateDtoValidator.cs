using FluentValidation;
using chatservice.Dto;

namespace chatservice.Validators
{
    public class ChatCreateDtoValidator : AbstractValidator<ChatCreateDto>
    {
        public ChatCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(1)
                .MaximumLength(100);
        }
    }
}
