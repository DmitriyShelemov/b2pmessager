﻿using queuemessagelibrary.MessageBus.Interfaces;
using System.Text.Json.Serialization;

namespace tenantservice.Dto
{
    public class TenantCreateDto : TenantDto, IBaseEvent<CrudActionType>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public override Guid TenantUID { get; set; }

        public CrudActionType EventType { get; set; }
    }
}
