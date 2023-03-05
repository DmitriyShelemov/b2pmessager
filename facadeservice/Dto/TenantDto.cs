namespace facadeservice.Dto
{
    public class TenantDto
    {
        public virtual Guid TenantUID { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }
    }
}
