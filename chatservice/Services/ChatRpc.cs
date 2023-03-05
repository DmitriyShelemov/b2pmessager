using chatservice.Dto;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;

namespace chatservice.Services
{
    public class ChatRpc : MessageRpc<CrudActionType>
    {
        public ChatRpc(
            IMessageConnection connection,
            IServiceScopeFactory scopeFactory)
            : base(connection, scopeFactory, "chatrpc")
        {
        }
    }
}
