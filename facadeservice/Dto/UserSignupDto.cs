using queuemessagelibrary.MessageBus.Interfaces;
namespace facadeservice.Dto
{
    public class UserSignupDto : UserCreateDto
    {
        public string? Password { get; set; }
    }
}
