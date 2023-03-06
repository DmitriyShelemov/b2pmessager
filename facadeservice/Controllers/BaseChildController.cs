using FluentValidation;
using facadeservice.Dto;
using facadeservice.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace facadeservice.Controllers
{
    public abstract class BaseChildController<CreateDto, UpdateDto, ReadDto, TService> : BaseTenantController
        where ReadDto : class
        where CreateDto : class
        where UpdateDto : class
        where TService : ICrudService<ReadDto, CreateDto, UpdateDto>
    {

        private readonly TService _service;
        private readonly IValidator<CreateDto> _validator;
        private readonly IValidator<UpdateDto> _updateValidator;

        public BaseChildController(
            TService service,
            IValidator<CreateDto> validator,
            IValidator<UpdateDto> updateValidator)
        {
            _service = service;
            _validator = validator;
            _updateValidator = updateValidator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
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

        public virtual async Task<IActionResult> GetAll(string parentId, uint? take, uint? skip)
        {
            if (!Guid.TryParse(parentId, out var uid))
            {
                return BadRequest();
            }

            var entities = await _service.GetAllAsync(uid, PageOptionsDto.Build(take, skip));
            return Ok(entities);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDto entity)
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
            return base.CreatedAtAction(nameof(Get), new
            {
                id = GetUID(entity).ToString("D"),
                tenantUID = GetTenantUID(entity).ToString("D")
            }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateDto entity)
        {
            if (!Guid.TryParse(id, out var uid))
            {
                return BadRequest();
            }

            var result = await _updateValidator.ValidateAsync(entity);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
            }

            SetUID(entity, uid);
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

        protected abstract Guid GetUID(CreateDto entity);

        protected abstract void SetUID(UpdateDto entity, Guid uid);

        protected abstract Guid GetTenantUID(CreateDto entity);
    }
}
