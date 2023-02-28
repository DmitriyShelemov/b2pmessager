using System.Text.Json.Serialization;

namespace tenantservice.Dto
{
    public class TenantCreateDto : TenantDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public override Guid TenantUID { get; set; }
    }
}
