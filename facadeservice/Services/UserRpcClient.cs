using facadeservice.Dto;
using facadeservice.Services.Interfaces;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;

namespace facadeservice.Services
{
    public class UserRpcClient : MessageRpcClient, IRpcClient<UserDto>
    {
        public UserRpcClient(IMessageConnection connection) 
            : base(connection, "userrpc")
        {
        }
    }
}
