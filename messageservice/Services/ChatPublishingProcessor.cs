using messageservice.Dto;
using messageservice.Services.Interfaces;
using queuemessagelibrary.MessageBus.Interfaces;

namespace messageservice.Services
{
    public class ChatPublishingProcessor : IEventProcessor<ChatDto>
    {
        private readonly IGenericRepository<ChatDto> _repository;

        public ChatPublishingProcessor(IGenericRepository<ChatDto> repository)
        {
            _repository = repository;
        }

        public async Task<string?> ProcessEvent(ChatDto? message, string src)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (message.Deleted)
            {
                var old = await _repository.GetByIdAsync(message.ChatUID);
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
