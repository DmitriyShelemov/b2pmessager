using facadeservice.Dto;

namespace facadeservice.Services.Interfaces
{
    public interface ITenantService : ICrudService<TenantDto, TenantCreateDto, TenantCreateDto>
    {
    }
}
