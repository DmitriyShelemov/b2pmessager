using System.Text.Json.Serialization;

namespace facadeservice.Dto
{
    public class TenantCreateDto : TenantDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public override Guid TenantUID { get; set; }
    }
}
