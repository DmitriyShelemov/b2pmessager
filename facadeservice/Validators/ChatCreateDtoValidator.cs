using FluentValidation;
using facadeservice.Dto;

namespace facadeservice.Validators
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
