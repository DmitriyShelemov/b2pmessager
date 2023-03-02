using facadeservice.Dto;
using facadeservice.Services.Interfaces;

namespace facadeservice.Services
{
    public class ChatService : IChatService
    {
        private readonly IGenericRepository<ChatDto> _repository;
        private readonly ITenantResolver _tenantResolver;

        public ChatService(
            IGenericRepository<ChatDto> repository,
            ITenantResolver tenantResolver)
        {
            _repository = repository;
            _tenantResolver = tenantResolver;
        }

        public async Task<IEnumerable<ChatDto>> GetAllAsync(PageOptionsDto opts)
        {
            //    if (!await _context.CanReadBaccountAsync())
            //        throw new UnauthorizedAccessException();

            return await _repository.GetAllAsync(opts);
        }

        public async Task<bool> AddAsync(ChatCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (!await _context.CanCreateBaccountAsync())
            //    throw new UnauthorizedAccessException();

            entity.ChatUID = Guid.NewGuid();
            entity.TenantUID = _tenantResolver.GetTenantUID();
            return await _repository.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(ChatCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (!await _context.CanEditBaccountAsync())
            //    throw new UnauthorizedAccessException();

            var old = await _repository.GetByIdAsync(entity.ChatUID);
            if (old != null)
            {
                old.Name = entity.Name;

                return await _repository.UpdateAsync(old);
            }

            return false;
        }

        public async Task<ChatDto> GetByIdAsync(Guid id)
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

        public Task<IEnumerable<ChatDto>> GetAllAsync(Guid parentId, PageOptionsDto opts)
        {
            throw new NotImplementedException();
        }
    }
}
