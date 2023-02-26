using MessageService.WebApi.Dto;

namespace MessageService.WebApi.Services.Interfaces
{
    public interface IMessageService : ICrudService<MessageDto, MessageCreateDto, MessageCreateDto>
    {
    }
}
