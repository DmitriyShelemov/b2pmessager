using facadeservice.Dto;
using facadeservice.Services.Interfaces;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;

namespace facadeservice.Services
{
    public class TenantRpcClient : MessageRpcClient, IRpcClient<TenantDto>
    {
        public TenantRpcClient(IMessageConnection connection) 
            : base(connection, "tenantrpc")
        {
        }
    }
}
