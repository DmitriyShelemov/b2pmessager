using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;
using tenantservice.Dto;

namespace tenantservice.Services
{
    public class TenantPublisher : MessagePublisher<TenantDto>
    {
        public TenantPublisher(IMessageConnection connection) 
            : base(connection, "tenantpublisher")
        {
        }
    }
}
