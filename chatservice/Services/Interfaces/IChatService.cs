using chatservice.Dto;

namespace chatservice.Services.Interfaces
{
    public interface IChatService : ICrudService<ChatDto, ChatCreateDto, ChatCreateDto>
    {
    }
}
