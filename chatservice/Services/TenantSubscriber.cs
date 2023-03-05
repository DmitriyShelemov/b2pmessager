using chatservice.Dto;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;

namespace chatservice.Services
{
    public class TenantSubscriber : MessageSubscriber<TenantDto>
    {
        public TenantSubscriber(IMessageConnection connection, IEventProcessor<TenantDto> eventProcessor) 
            : base(connection, eventProcessor, "tenantpublisher")
        {
        }
    }
}
