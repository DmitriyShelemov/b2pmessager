using facadeservice.Dto;
using facadeservice.Services.Interfaces;

namespace facadeservice.Services
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
            //    if (!await _context.CanReadBaccountAsync())
            //        throw new UnauthorizedAccessException();

            return await _repository.GetAllAsync(opts);
        }

        public async Task<bool> AddAsync(TenantCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (!await _context.CanCreateBaccountAsync())
            //    throw new UnauthorizedAccessException();

            entity.TenantUID = Guid.NewGuid();
            return await _repository.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(TenantCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (!await _context.CanEditBaccountAsync())
            //    throw new UnauthorizedAccessException();

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
            //if (!await _context.CanReadBaccountAsync())
            //    throw new UnauthorizedAccessException();

            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            //if (!await _context.CanEditBaccountAsync())
            //    throw new UnauthorizedAccessException();

            var old = await _repository.GetByIdAsync(id);
            if (old != null)
            {
                old.Deleted = true;
                return await _repository.UpdateAsync(old);
            }

            return false;
        }

        public Task<IEnumerable<TenantDto>> GetAllAsync(Guid parentId, PageOptionsDto opts)
        {
            throw new NotImplementedException();
        }
    }
}
