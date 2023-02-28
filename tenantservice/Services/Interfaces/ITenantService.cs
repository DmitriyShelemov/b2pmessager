using tenantservice.Dto;

namespace tenantservice.Services.Interfaces
{
    public interface ITenantService : ICrudService<TenantDto, TenantCreateDto, TenantCreateDto>
    {
    }
}
