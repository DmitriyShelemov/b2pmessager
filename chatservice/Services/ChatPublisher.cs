using chatservice.Dto;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;

namespace chatservice.Services
{
    public class ChatPublisher : MessagePublisher<ChatDto>
    {
        public ChatPublisher(IMessageConnection connection)
            : base(connection, "chatpublisher")
        {
        }
    }
}
