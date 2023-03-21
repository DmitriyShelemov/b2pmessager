using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;
using userservice.Dto;

namespace userservice.Services
{
    public class UserRpc : MessageRpc<CrudActionType>
    {
        public UserRpc(
            IMessageConnection connection, 
            IServiceScopeFactory scopeFactory) 
            : base(connection, scopeFactory, "userrpc")
        {
        }
    }
}
