using queuemessagelibrary.MessageBus.Interfaces;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace facadeservice.Dto
{
    public class MessageUpdateDto : MessageDto, IBaseEvent<CrudActionType>
    {
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public override Guid MessageUID { get; set; }

        [JsonIgnore]
        public override Guid ChatUID { get; set; }

        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid TenantUID { get; set; }

        [DefaultValue(CrudActionType.None)]
        public CrudActionType EventType { get; set; }
    }
}
