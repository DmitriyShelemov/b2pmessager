using facadeservice.Dto;
using facadeservice.Services.Interfaces;

namespace facadeservice.Services
{
    public class ChatService : IChatService
    {
        private readonly IRpcClient<ChatDto> _rpcClient;
        private readonly ITenantResolver _tenantResolver;

        public ChatService(
            ITenantResolver tenantResolver, IRpcClient<ChatDto> rpcClient)
        {
            _tenantResolver = tenantResolver;
            _rpcClient = rpcClient;
        }

        public async Task<IEnumerable<ChatDto>> GetAllAsync(PageOptionsDto opts)
        {
            //    if (!await _context.CanReadBaccountAsync())
            //        throw new UnauthorizedAccessException();

            opts.TenantUID = _tenantResolver.GetTenantUID();
            opts.EventType = CrudActionType.Gets;
            return await _rpcClient.RequestAsync<PageOptionsDto, IEnumerable<ChatDto>>(opts) ?? Enumerable.Empty<ChatDto>();
        }

        public async Task<bool> AddAsync(ChatCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (!await _context.CanCreateBaccountAsync())
            //    throw new UnauthorizedAccessException();

            entity.TenantUID = _tenantResolver.GetTenantUID();
            entity.EventType = CrudActionType.Create;
            var created = await _rpcClient.RequestAsync<ChatCreateDto, ChatDto>(entity);
            if (created == null)
                return false;

            entity.ChatUID = created.ChatUID;
            return true;
        }

        public async Task<bool> UpdateAsync(ChatCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (!await _context.CanEditBaccountAsync())
            //    throw new UnauthorizedAccessException();

            entity.TenantUID = _tenantResolver.GetTenantUID();
            entity.EventType = CrudActionType.Update;
            var updated = await _rpcClient.RequestAsync<ChatCreateDto, BoolDto>(entity);
            return updated?.Done ?? false;
        }

        public async Task<ChatDto?> GetByIdAsync(Guid id)
        {
            //if (!await _context.CanReadBaccountAsync())
            //    throw new UnauthorizedAccessException();

            var guidDto = new GuidDto
            {
                Id = id,
                EventType = CrudActionType.Get,
                TenantUID = _tenantResolver.GetTenantUID()
            };

            return await _rpcClient.RequestAsync<GuidDto, ChatDto>(guidDto);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            //if (!await _context.CanEditBaccountAsync())
            //    throw new UnauthorizedAccessException();

            var guidDto = new GuidDto
            {
                Id = id,
                EventType = CrudActionType.Delete,
                TenantUID = _tenantResolver.GetTenantUID()
            };
            var deleted = await _rpcClient.RequestAsync<GuidDto, BoolDto>(guidDto);
            return deleted?.Done ?? false;
        }

        public Task<IEnumerable<ChatDto>> GetAllAsync(Guid parentId, PageOptionsDto opts)
        {
            throw new NotImplementedException();
        }
    }
}
