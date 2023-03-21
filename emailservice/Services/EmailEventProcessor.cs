using FluentValidation;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;
using System.Text.Json;
using emailservice.Dto;
using emailservice.Services.Interfaces;

namespace emailservice.Services
{
    public class EmailEventProcessor : IEventProcessor<BaseEvent<CrudActionType>>
    {
        private readonly IEmailService _service;
        private readonly IValidator<VerifyEmailDto> _validator;

        public EmailEventProcessor(
            IEmailService service,
            IValidator<VerifyEmailDto> validator)
        {
            _service = service;
            _validator = validator;
        }

        public async Task<string?> ProcessEvent(BaseEvent<CrudActionType>? message, string src)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            switch (message.EventType)
            {
                case CrudActionType.Verify:
                    return await CallService<VerifyEmailDto>(src, async (m) => {

                        var result = await _validator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        return new BoolDto { Done = await _service.SendVerificationAsync(m) };
                    });
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task<string> CallService<T>(string src, Func<T, Task<object>> act) where T : IBaseEvent<CrudActionType>
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));
            if (act == null)
                throw new ArgumentNullException(nameof(act));

            var getRequest = JsonSerializer.Deserialize<T>(src);
            if (getRequest == null)
                throw new ArgumentNullException(nameof(getRequest));

            var getResponse = await act(getRequest);
            return JsonSerializer.Serialize(getResponse);
        }
    }
}
