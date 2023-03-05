using queuemessagelibrary.MessageBus.Interfaces;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace facadeservice.Dto
{
    public class TenantCreateDto : TenantDto, IBaseEvent<CrudActionType>
    {
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public override Guid TenantUID { get; set; }

        [DefaultValue(CrudActionType.None)]
        public CrudActionType EventType { get; set; }
    }
}
