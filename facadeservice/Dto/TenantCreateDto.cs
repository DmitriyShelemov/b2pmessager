using queuemessagelibrary.MessageBus.Interfaces;
using System.Text.Json.Serialization;

namespace facadeservice.Dto
{
    public class TenantCreateDto : TenantDto, IBaseEvent<CrudActionType>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public override Guid TenantUID { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public CrudActionType EventType { get; set; }
    }
}
