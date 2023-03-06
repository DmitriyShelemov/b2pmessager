using messageservice.Dto;

namespace messageservice.Services.Interfaces
{
    public interface IMessageService : ICrudService<MessageDto, MessageCreateDto, MessageCreateDto>
    {
    }
}
