using queuemessagelibrary.MessageBus.Interfaces;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace facadeservice.Dto
{
    public class UserCreateDto : UserDto
    {
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public override Guid UserUID { get; set; }

        [JsonIgnore]
        public override bool Activated { get; set; }
    }
}
