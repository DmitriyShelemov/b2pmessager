using messageservice.Dto;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;

namespace messageservice.Services
{
    public class ChatSubscriber : MessageSubscriber<ChatDto>
    {
        public ChatSubscriber(
            IMessageConnection connection,
            IServiceScopeFactory scopeFactory)
            : base(connection, scopeFactory, "chatpublisher")
        {
        }
    }
}
