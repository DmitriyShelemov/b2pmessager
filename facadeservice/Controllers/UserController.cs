using FluentValidation;
using facadeservice.Dto;
using facadeservice.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace facadeservice.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IValidator<UserCreateDto> _validator;
        private readonly IValidator<UserUpdateDto> _updateValidator;

        public UserController(
            IUserService service, 
            IValidator<UserCreateDto> validator, 
            IValidator<UserUpdateDto> updateValidator)
        {
            _service = service;
            _validator = validator;
            _updateValidator = updateValidator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
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
        public async Task<IActionResult> GetUsers(uint? take, uint? skip)
        {
            var entities = await _service.GetAllAsync(PageOptionsDto.Build(take, skip));
            return Ok(entities);
        }

        [HttpPost(BaseTenantController.TenantRoute + "[controller]")]
        public async Task<IActionResult> CreateUser(UserCreateDto entity)
        {
            var result = await _validator.ValidateAsync(entity);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
            }

            if (!await _service.AddAsync(entity, GenerateVerificationTemplate()))
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetUser), new
            {
                id = entity.UserUID.ToString("D")
            }, entity);
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> SignupUser(UserSignupDto entity)
        {
            var result = await _validator.ValidateAsync(entity);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
            }

            if (!await _service.SignupAsync(entity, GenerateVerificationTemplate()))
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetUser), new
            {
                id = entity.UserUID.ToString("D")
            }, entity);
        }

        [HttpGet("{id}/Activate")]
        [AllowAnonymous]
        public async Task<IActionResult> ActivateUser(string id, string verificationKey)
        {
            if (!Guid.TryParse(id, out var uid))
            {
                return BadRequest();
            }

            if (!await _service.ActivateAsync(new UserActivateDto { Id = uid, VerificationKey = verificationKey }))
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserUpdateDto entity)
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

            entity.UserUID = uid;
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

        private string? GenerateVerificationTemplate() => Url.Action(nameof(ActivateUser), new { id = "{0}", verificationKey = "{1}" });
    }
}