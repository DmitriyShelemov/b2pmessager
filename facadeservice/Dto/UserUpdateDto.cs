using queuemessagelibrary.MessageBus.Interfaces;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace facadeservice.Dto
{
    public class UserUpdateDto : UserDto, IBaseEvent<CrudActionType>
    {
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public override Guid UserUID { get; set; }

        [DefaultValue(CrudActionType.None)]
        public CrudActionType EventType { get; set; }

        [JsonIgnore]
        public override bool Activated { get; set; }

        [JsonIgnore]
        public override string? Name { get; set; }

        [JsonIgnore]
        public override string? Email { get; set; }

        public string? Password { get; set; }
    }
}
