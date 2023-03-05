using tenantservice.Dto;
using tenantservice.Services.Interfaces;

namespace tenantservice.Services
{
    public class TenantService : ITenantService
    {
        private readonly IGenericRepository<TenantDto> _repository;

        public TenantService(
            IGenericRepository<TenantDto> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TenantDto>> GetAllAsync(PageOptionsDto opts)
        {
            return await _repository.GetAllAsync(opts);
        }

        public async Task<bool> AddAsync(TenantCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.TenantUID = Guid.NewGuid();
            return await _repository.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(TenantCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var old = await _repository.GetByIdAsync(entity.TenantUID);
            if (old != null)
            {
                old.Name = entity.Name;
                old.Email = entity.Email;

                return await _repository.UpdateAsync(old);
            }

            return false;
        }

        public async Task<TenantDto> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var old = await _repository.GetByIdAsync(id);
            if (old != null)
            {
                old.Deleted = true;
                return await _repository.UpdateAsync(old);
            }

            return false;
        }
    }
}
