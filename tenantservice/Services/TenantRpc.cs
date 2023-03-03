using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;
using tenantservice.Dto;

namespace tenantservice.Services
{
    public class TenantRpc : MessageRpc<CrudActionType>
    {
        public TenantRpc(
            IMessageConnection connection, 
            IServiceScopeFactory scopeFactory) 
            : base(connection, scopeFactory, "tenantrpc")
        {
        }
    }
}
