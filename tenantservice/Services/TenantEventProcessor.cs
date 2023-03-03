using FluentValidation;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;
using System.Text.Json;
using tenantservice.Dto;
using tenantservice.Services.Interfaces;

namespace tenantservice.Services
{
    public class TenantEventProcessor : IEventProcessor<BaseEvent<CrudActionType>>
    {
        private readonly ITenantService _service;
        private readonly IValidator<TenantCreateDto> _validator;
        private readonly IMessagePublisher<TenantDto> _publisher;

        public TenantEventProcessor(
            ITenantService service, 
            IMessagePublisher<TenantDto> publisher, 
            IValidator<TenantCreateDto> validator)
        {
            _service = service;
            _publisher = publisher;
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
                case CrudActionType.Get:
                    return await CallService<GuidDto>(src, async (m) => { return await _service.GetByIdAsync(m.Id); });
                case CrudActionType.Gets:
                    return await CallService<PageOptionsDto>(src, async (m) => { return await _service.GetAllAsync(m); });
                case CrudActionType.Create:
                    return await CallService<TenantCreateDto>(src, async (m) => {

                        var result = await _validator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        var done = await _service.AddAsync(m);
                        if (done)
                        {
                            var created = new TenantDto
                            {
                                Name = m.Name,
                                TenantUID = m.TenantUID
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
                    return await CallService<TenantCreateDto>(src, async (m) => {

                        var result = await _validator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        return new BoolDto { Done = await _service.UpdateAsync(m) };
                    });
                case CrudActionType.Delete:
                    return await CallService<GuidDto>(src, async (m) => { 
                        var done = await _service.DeleteAsync(m.Id);
                        if (done)
                        {
                            _publisher.Publish(new TenantDto
                            {
                                TenantUID = m.Id,
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
