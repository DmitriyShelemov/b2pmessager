using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;
using userservice.Dto;

namespace userservice.Services
{
    public class UserPublisher : MessagePublisher<UserDto>
    {
        public UserPublisher(IMessageConnection connection) 
            : base(connection, "userpublisher")
        {
        }
    }
}
