using facadeservice.Dto;
using facadeservice.Services.Interfaces;

namespace facadeservice.Services
{
    public class UserService : IUserService
    {
        private readonly IRpcClient<UserDto> _rpcClient;
        private readonly ITenantResolver _tenantResolver;

        public UserService(
            ITenantResolver tenantResolver, IRpcClient<UserDto> rpcClient)
        {
            _tenantResolver = tenantResolver;
            _rpcClient = rpcClient;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(PageOptionsDto opts)
        {
            //    if (!await _context.CanReadBaccountAsync())
            //        throw new UnauthorizedAccessException();

            opts.TenantUID = _tenantResolver.GetTenantUID();
            opts.EventType = CrudActionType.Gets;
            return await _rpcClient.RequestAsync<PageOptionsDto, IEnumerable<UserDto>>(opts) ?? Enumerable.Empty<UserDto>();
        }

        public async Task<bool> AddAsync(UserCreateDto entity, string verificationUrl)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (!await _context.CanCreateBaccountAsync())
            //    throw new UnauthorizedAccessException();

            var created = await _rpcClient.RequestAsync<UserCreateQueueDto, UserDto>(new UserCreateQueueDto
            {
                Name = entity.Name,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                EventType = CrudActionType.Create,
                VerificationUrl = verificationUrl,
                TenantUID = _tenantResolver.GetTenantUID()
            });
            if (created == null)
                return false;

            entity.UserUID = created.UserUID;
            return true;
        }

        public async Task<bool> SignupAsync(UserSignupDto entity, string verificationUrl)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (!await _context.CanCreateBaccountAsync())
            //    throw new UnauthorizedAccessException();

            var created = await _rpcClient.RequestAsync<UserCreateQueueDto, UserDto>(new UserCreateQueueDto
            {
                Name = entity.Name,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                EventType = CrudActionType.Signup,
                Password = entity.Password,
                VerificationUrl = verificationUrl
            });
            if (created == null)
                return false;

            entity.UserUID = created.UserUID;
            return true;
        }

        public async Task<bool> ActivateAsync(UserActivateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (!await _context.CanEditBaccountAsync())
            //    throw new UnauthorizedAccessException();

            entity.EventType = CrudActionType.Activate;
            var updated = await _rpcClient.RequestAsync<UserActivateDto, BoolDto>(entity);
            return updated?.Done ?? false;
        }

        public async Task<bool> UpdateAsync(UserUpdateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (!await _context.CanEditBaccountAsync())
            //    throw new UnauthorizedAccessException();

            entity.EventType = CrudActionType.Update;
            var updated = await _rpcClient.RequestAsync<UserUpdateDto, BoolDto>(entity);
            return updated?.Done ?? false;
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            //if (!await _context.CanReadBaccountAsync())
            //    throw new UnauthorizedAccessException();

            var guidDto = new GuidWithTenantDto
            {
                Id = id,
                EventType = CrudActionType.Get,
                TenantUID = _tenantResolver.GetTenantUID()
            };

            return await _rpcClient.RequestAsync<GuidDto, UserDto>(guidDto);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            //if (!await _context.CanEditBaccountAsync())
            //    throw new UnauthorizedAccessException();

            var guidDto = new GuidWithTenantDto
            {
                Id = id,
                EventType = CrudActionType.Delete,
                TenantUID = _tenantResolver.GetTenantUID()
            };
            var deleted = await _rpcClient.RequestAsync<GuidDto, BoolDto>(guidDto);
            return deleted?.Done ?? false;
        }

        public Task<IEnumerable<UserDto>> GetAllAsync(Guid parentId, PageOptionsDto opts)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddAsync(UserCreateDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
