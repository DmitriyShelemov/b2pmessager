using messageservice.Dto;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;

namespace messageservice.Services
{
    public class TenantSubscriber : MessageSubscriber<TenantDto>
    {
        public TenantSubscriber(
            IMessageConnection connection,
            IServiceScopeFactory scopeFactory)
            : base(connection, scopeFactory, "tenantpublisher")
        {
        }
    }
}
