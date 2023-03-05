namespace chatservice.Services.Interfaces
{
    public interface ITenantResolver
    {
        Guid GetTenantUID();

        void SetTenantUID(Guid uid);
    }
}
