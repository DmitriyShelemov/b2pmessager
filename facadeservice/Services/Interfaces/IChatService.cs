using facadeservice.Dto;

namespace facadeservice.Services.Interfaces
{
    public interface IChatService : ICrudService<ChatDto, ChatCreateDto, ChatCreateDto>
    {
    }
}
