using FluentValidation;
using facadeservice.Dto;
using facadeservice.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace facadeservice.Controllers
{
    public class MessageController
        : BaseChildController<MessageCreateDto, MessageUpdateDto, MessageDto, IMessageService>
    {
        public MessageController(
            IMessageService service,
            IValidator<MessageCreateDto> validator,
            IValidator<MessageUpdateDto> updateValidator)
            : base(service, validator, updateValidator)
        {
        }

        protected override Guid GetUID(MessageCreateDto entity) => entity.MessageUID;

        protected override void SetUID(MessageUpdateDto entity, Guid uid) => entity.MessageUID = uid;

        protected override Guid GetTenantUID(MessageCreateDto entity) => entity.TenantUID;

        [HttpGet(TenantRoute + "Chat/{chatId}/[controller]")]
        public override async Task<IActionResult> GetAll(string chatId, uint? take, uint? skip)
        {
            return await base.GetAll(chatId, take, skip);
        }
    }
}