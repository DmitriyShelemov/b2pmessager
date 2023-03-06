using messageservice.Dto;
using messageservice.Services.Interfaces;

namespace messageservice.Services
{
    public class MessageService : IMessageService
    {
        private readonly IGenericRepository<MessageDto> _repository;
        private readonly ITenantResolver _tenantResolver;

        public MessageService(
            IGenericRepository<MessageDto> repository,
            ITenantResolver tenantResolver)
        {
            _repository = repository;
            _tenantResolver = tenantResolver;
        }

        public async Task<IEnumerable<MessageDto>> GetAllAsync(Guid parentId, PageOptionsDto opts)
        {
            return await _repository.GetAllAsync(parentId, opts);
        }

        public async Task<bool> AddAsync(MessageCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.MessageUID = Guid.NewGuid();
            entity.TenantUID = _tenantResolver.GetTenantUID();
            return await _repository.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(MessageCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var old = await _repository.GetByIdAsync(entity.MessageUID);
            if (old != null)
            {
                old.MessageText = entity.MessageText;

                return await _repository.UpdateAsync(old);
            }

            return false;
        }

        public async Task<MessageDto> GetByIdAsync(Guid id)
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
