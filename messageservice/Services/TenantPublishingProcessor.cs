using messageservice.Dto;
using messageservice.Services.Interfaces;
using queuemessagelibrary.MessageBus.Interfaces;

namespace messageservice.Services
{
    public class TenantPublishingProcessor : IEventProcessor<TenantDto>
    {
        private readonly IGenericRepository<TenantDto> _repository;

        public TenantPublishingProcessor(IGenericRepository<TenantDto> repository)
        {
            _repository = repository;
        }

        public async Task<string?> ProcessEvent(TenantDto? message, string src)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (message.Deleted)
            {
                var old = await _repository.GetByIdAsync(message.TenantUID);
                if (old != null)
                {
                    old.Deleted = message.Deleted;
                    await _repository.UpdateAsync(old);
                }
            }
            else
            {
                await _repository.AddAsync(message);
            }

            return null;
        }
    }
}
