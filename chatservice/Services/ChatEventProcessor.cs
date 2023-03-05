using FluentValidation;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;
using System.Text.Json;
using chatservice.Dto;
using chatservice.Services.Interfaces;

namespace chatservice.Services
{
    public class ChatEventProcessor : IEventProcessor<BaseEvent<CrudActionType>>
    {
        private readonly IChatService _service;
        private readonly IValidator<ChatCreateDto> _validator;
        private readonly IValidator<GuidDto> _guidValidator;
        private readonly IValidator<PageOptionsDto> _pagingValidator;
        private readonly IMessagePublisher<ChatDto> _publisher;
        private readonly ITenantResolver _tenantResolver;

        public ChatEventProcessor(
            IChatService service,
            IMessagePublisher<ChatDto> publisher,
            IValidator<ChatCreateDto> validator,
            IValidator<GuidDto> guidValidator,
            IValidator<PageOptionsDto> pagingValidator,
            ITenantResolver tenantResolver)
        {
            _service = service;
            _publisher = publisher;
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

            switch (message.EventType)
            {
                case CrudActionType.Get:
                    return await CallService<GuidDto>(src, async (m) => {

                        var result = await _guidValidator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        return await _service.GetByIdAsync(m.Id); 
                    });
                case CrudActionType.Gets:
                    return await CallService<PageOptionsDto>(src, async (m) => {

                        var result = await _pagingValidator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        return await _service.GetAllAsync(m); 
                    });
                case CrudActionType.Create:
                    return await CallService<ChatCreateDto>(src, async (m) =>
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
                            var created = new ChatDto
                            {
                                Name = m.Name,
                                ChatUID = m.ChatUID
                            };
                            _publisher.Publish(created);

                            return created;
                        }
                        else
                        {
                            return null;
                        }
                    });
                case CrudActionType.Update:
                    return await CallService<ChatCreateDto>(src, async (m) =>
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
                        if (done)
                        {
                            _publisher.Publish(new ChatDto
                            {
                                ChatUID = m.Id,
                                Deleted = done
                            });
                        }
                        else
                        {

                        }

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

            var getResponse = await act(getRequest);
            return JsonSerializer.Serialize(getResponse);
        }
    }
}
