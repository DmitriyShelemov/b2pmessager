using queuemessagelibrary.MessageBus.Interfaces;
using System.Text.Json.Serialization;

namespace userservice.Dto
{
    public class UserCreateDto : UserDto, IBaseEvent<CrudActionType>
    {
        public override Guid UserUID { get; set; }

        public Guid TenantUID { get; set; }

        public string? Password { get; set; }

        public string VerificationUrl { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CrudActionType EventType { get; set; }
    }
}
