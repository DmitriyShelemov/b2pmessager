using FluentValidation;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;
using System.Text.Json;
using messageservice.Dto;
using messageservice.Services.Interfaces;

namespace messageservice.Services
{
    public class MessageEventProcessor : IEventProcessor<BaseEvent<CrudActionType>>
    {
        private readonly IMessageService _service;
        private readonly IValidator<MessageCreateDto> _validator;
        private readonly IValidator<GuidDto> _guidValidator;
        private readonly IValidator<PageOptionsDto> _pagingValidator;
        private readonly ITenantResolver _tenantResolver;

        public MessageEventProcessor(
            IMessageService service,
            IValidator<MessageCreateDto> validator,
            IValidator<GuidDto> guidValidator,
            IValidator<PageOptionsDto> pagingValidator,
            ITenantResolver tenantResolver)
        {
            _service = service;
            _validator = validator;
            _guidValidator = guidValidator;
            _pagingValidator = pagingValidator;
            _tenantResolver = tenantResolver;
        }

        public async Task<string?> ProcessEvent(BaseEvent<CrudActionType>? message, string src)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            if (src == null)
                throw new ArgumentNullException(nameof(src));


            Console.WriteLine("Message came " + src);

            switch (message.EventType)
            {
                case CrudActionType.Get:
                    return await CallService<GuidDto>(src, async (m) =>
                    {

                        var result = await _guidValidator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        return await _service.GetByIdAsync(m.Id);
                    });
                case CrudActionType.Gets:
                    return await CallService<PageOptionsDto>(src, async (m) =>
                    {

                        var result = await _pagingValidator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        return await _service.GetAllAsync(m.ParentUID, m);
                    });
                case CrudActionType.Create:
                    return await CallService<MessageCreateDto>(src, async (m) =>
                    {

                        var result = await _validator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        var done = await _service.AddAsync(m);
                        if (done)
                        {
                            var created = new MessageDto
                            {
                                MessageUID = m.MessageUID,
                                ChatUID = m.ChatUID,
                                TenantUID = m.TenantUID,
                            };

                            return created;
                        }
                        else
                        {
                            return null;
                        }
                    });
                case CrudActionType.Update:
                    return await CallService<MessageCreateDto>(src, async (m) =>
                    {
                        var result = await _validator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        return new BoolDto { Done = await _service.UpdateAsync(m) };
                    });
                case CrudActionType.Delete:
                    return await CallService<GuidDto>(src, async (m) =>
                    {

                        var result = await _guidValidator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        var done = await _service.DeleteAsync(m.Id);
                        return new BoolDto { Done = done };
                    });
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task<string> CallService<T>(string src, Func<T, Task<object>> act) where T : IBaseEvent<CrudActionType>, ITenantContext
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));
            if (act == null)
                throw new ArgumentNullException(nameof(act));

            var getRequest = JsonSerializer.Deserialize<T>(src);
            if (getRequest == null)
                throw new ArgumentNullException(nameof(getRequest));

            _tenantResolver.SetTenantUID(getRequest.TenantUID);

            Console.WriteLine("Message in proc " + JsonSerializer.Serialize(getRequest));

            var getResponse = await act(getRequest);
            return JsonSerializer.Serialize(getResponse);
        }
    }
}
