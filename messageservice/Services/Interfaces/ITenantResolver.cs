namespace MessageService.WebApi.Services.Interfaces
{
    public interface ITenantResolver
    {
        Guid GetTenantUID();
    }
}
