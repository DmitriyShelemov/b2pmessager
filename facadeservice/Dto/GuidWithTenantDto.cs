
namespace facadeservice.Dto
{
    public class GuidWithTenantDto : GuidDto, ITenantContext
    {
        public Guid TenantUID { get; set; }
    }
}
