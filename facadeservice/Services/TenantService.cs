using facadeservice.Dto;
using facadeservice.Services.Interfaces;

namespace facadeservice.Services
{
    public class TenantService : ITenantService
    {
        private readonly IRpcClient<TenantDto> _rpcClient;

        public TenantService(IRpcClient<TenantDto> rpcClient)
        {
            _rpcClient = rpcClient;
        }

        public async Task<IEnumerable<TenantDto>> GetAllAsync(PageOptionsDto opts)
        {
            //    if (!await _context.CanReadBaccountAsync())
            //        throw new UnauthorizedAccessException();

            opts.EventType = CrudActionType.Gets;
            return await _rpcClient.RequestAsync<PageOptionsDto, IEnumerable<TenantDto>>(opts) ?? Enumerable.Empty<TenantDto>();
        }

        public async Task<bool> AddAsync(TenantCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (!await _context.CanCreateBaccountAsync())
            //    throw new UnauthorizedAccessException();

            entity.EventType = CrudActionType.Create;
            var created = await _rpcClient.RequestAsync<TenantCreateDto, TenantDto>(entity);
            if (created == null)
                return false;

            entity.TenantUID = created.TenantUID;
            return true;
        }

        public async Task<bool> UpdateAsync(TenantCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (!await _context.CanEditBaccountAsync())
            //    throw new UnauthorizedAccessException();

            entity.EventType = CrudActionType.Update;
            var updated = await _rpcClient.RequestAsync<TenantCreateDto, BoolDto>(entity);
            return updated?.Done ?? false;
        }

        public async Task<TenantDto?> GetByIdAsync(Guid id)
        {
            //if (!await _context.CanReadBaccountAsync())
            //    throw new UnauthorizedAccessException();

            var guidDto = new GuidDto
            {
                Id = id,
                EventType = CrudActionType.Get
            };

            return await _rpcClient.RequestAsync<GuidDto, TenantDto>(guidDto);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            //if (!await _context.CanEditBaccountAsync())
            //    throw new UnauthorizedAccessException();

            var guidDto = new GuidDto
            {
                Id = id,
                EventType = CrudActionType.Delete
            };

            var deleted = await _rpcClient.RequestAsync<GuidDto, BoolDto>(guidDto);
            return deleted?.Done ?? false;
        }

        public Task<IEnumerable<TenantDto>> GetAllAsync(Guid parentId, PageOptionsDto opts)
        {
            throw new NotImplementedException();
        }
    }
}
