using queuemessagelibrary.MessageBus.Interfaces;
using System.ComponentModel;

namespace facadeservice.Dto
{
    public class UserCreateQueueDto : UserSignupDto, IBaseEvent<CrudActionType>
    {
        public string VerificationUrl { get; set; } = string.Empty;

        public Guid TenantUID { get; set; }

        [DefaultValue(CrudActionType.None)]
        public CrudActionType EventType { get; set; }
    }
}
