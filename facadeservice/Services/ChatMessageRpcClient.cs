using facadeservice.Dto;
using facadeservice.Services.Interfaces;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;

namespace facadeservice.Services
{
    public class ChatMessageRpcClient : MessageRpcClient, IRpcClient<MessageDto>
    {
        public ChatMessageRpcClient(IMessageConnection connection) 
            : base(connection, "messagerpc")
        {
        }
    }
}
