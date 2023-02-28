using FluentValidation;
using chatservice.Dto;
using chatservice.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace chatservice.Controllers
{
    public class ChatController : BaseTenantController
    {
        private readonly IChatService _service;
        private readonly IValidator<ChatCreateDto> _validator;

        public ChatController(IChatService service, IValidator<ChatCreateDto> validator)
        {
            _service = service;
            _validator = validator;

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChat(string id)
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
        public async Task<IActionResult> GetChats(uint? take, uint? skip)
        {
            var entities = await _service.GetAllAsync(PageOptionsDto.Build(take, skip));
            return Ok(entities);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat(ChatCreateDto entity)
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
            return CreatedAtAction(nameof(GetChat), new
            {
                id = entity.ChatUID.ToString("D"),
                tenantUID = entity.TenantUID.ToString("D")
            }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChat(string id, ChatCreateDto entity)
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

            entity.ChatUID = uid;
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