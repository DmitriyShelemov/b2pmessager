using chatservice.Dto;
using chatservice.Services.Interfaces;
using System.Text.Json;

namespace chatservice.Services
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
            return await _repository.GetAllAsync(opts);
        }

        public async Task<bool> AddAsync(ChatCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.ChatUID = Guid.NewGuid();
            entity.TenantUID = _tenantResolver.GetTenantUID();
            
            Console.WriteLine("Message before add " + JsonSerializer.Serialize(entity));
            return await _repository.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(ChatCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

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
