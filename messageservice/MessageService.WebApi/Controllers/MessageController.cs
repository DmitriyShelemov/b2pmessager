using FluentValidation;
using MessageService.WebApi.Dto;
using MessageService.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.WebApi.Controllers
{
    public class MessageController : BaseTenantController
    {
        private readonly IMessageService _service;
        private readonly IValidator<MessageCreateDto> _validator;

        public MessageController(IMessageService service, IValidator<MessageCreateDto> validator)
        {
            _service = service;
            _validator = validator;

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(string id)
        {
            if (!Guid.TryParse(id, out var uid))
            {
                return BadRequest();
            }

            var entity = await _service.GetByIdAsync(uid);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(uint? take, uint? skip)
        {
            var entities = await _service.GetAllAsync(PageOptionsDto.Build(take, skip));
            return Ok(entities);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(MessageCreateDto entity)
        {
            var result = await _validator.ValidateAsync(entity);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
            }

            if (!await _service.AddAsync(entity))
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetMessage), new
            {
                id = entity.MessageUID.ToString("D"),
                teamId = entity.TenantUID.ToString("D")
            }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(string id, MessageCreateDto entity)
        {
            if (!Guid.TryParse(id, out var uid))
            {
                return BadRequest();
            }

            var result = await _validator.ValidateAsync(entity);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
            }

            entity.MessageUID = uid;
            if (!await _service.UpdateAsync(entity))
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out var uid))
            {
                return BadRequest();
            }

            if (!await _service.DeleteAsync(uid))
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}