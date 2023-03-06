using messageservice.Dto;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;

namespace messageservice.Services
{
    public class ChatMessageRpc : MessageRpc<CrudActionType>
    {
        public ChatMessageRpc(
            IMessageConnection connection,
            IServiceScopeFactory scopeFactory)
            : base(connection, scopeFactory, "messagerpc")
        {
        }
    }
}
