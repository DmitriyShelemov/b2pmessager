using facadeservice.Dto;
using facadeservice.Services.Interfaces;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;

namespace facadeservice.Services
{
    public class ChatRpcClient : MessageRpcClient, IRpcClient<ChatDto>
    {
        public ChatRpcClient(IMessageConnection connection) 
            : base(connection, "chatrpc")
        {
        }
    }
}
