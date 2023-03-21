using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;
using emailservice.Dto;

namespace emailservice.Services
{
    public class EmailRpc : MessageRpc<CrudActionType>
    {
        public EmailRpc(
            IMessageConnection connection, 
            IServiceScopeFactory scopeFactory) 
            : base(connection, scopeFactory, "emailrpc")
        {
        }
    }
}
