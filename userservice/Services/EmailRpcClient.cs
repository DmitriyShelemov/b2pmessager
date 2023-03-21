using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;
using userservice.Dto;
using userservice.Services.Interfaces;

namespace userservice.Services
{
    public class EmailRpcClient : MessageRpcClient, IRpcClient<VerifyEmailDto>
    {
        public EmailRpcClient(IMessageConnection connection)
            : base(connection, "emailrpc")
        {
        }
    }
}
