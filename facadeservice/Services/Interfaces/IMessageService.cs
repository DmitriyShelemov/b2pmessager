using facadeservice.Dto;

namespace facadeservice.Services.Interfaces
{
    public interface IMessageService : ICrudService<MessageDto, MessageCreateDto, MessageUpdateDto>
    {
    }
}
