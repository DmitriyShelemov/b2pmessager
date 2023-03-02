using FluentValidation;
using facadeservice.Dto;
using facadeservice.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace facadeservice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _service;
        private readonly IValidator<TenantCreateDto> _validator;

        public TenantController(ITenantService service, IValidator<TenantCreateDto> validator)
        {
            _service = service;
            _validator = validator;

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTenant(string id)
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
        public async Task<IActionResult> GetTenants(uint? take, uint? skip)
        {
            var entities = await _service.GetAllAsync(PageOptionsDto.Build(take, skip));
            return Ok(entities);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant(TenantCreateDto entity)
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
            return CreatedAtAction(nameof(GetTenant), new
            {
                id = entity.TenantUID.ToString("D")
            }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTenant(string id, TenantCreateDto entity)
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

            entity.TenantUID = uid;
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