using facadeservice.Dto;
using facadeservice.Services.Interfaces;

namespace facadeservice.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRpcClient<MessageDto> _rpcClient;
        private readonly ITenantResolver _tenantResolver;

        public MessageService(
            ITenantResolver tenantResolver, IRpcClient<MessageDto> rpcClient)
        {
            _tenantResolver = tenantResolver;
            _rpcClient = rpcClient;
        }

        public async Task<IEnumerable<MessageDto>> GetAllAsync(Guid parentId, PageOptionsDto opts)
        {
            //    if (!await _context.CanReadBaccountAsync())
            //        throw new UnauthorizedAccessException();

            opts.TenantUID = _tenantResolver.GetTenantUID();
            opts.ParentUID = parentId;
            opts.EventType = CrudActionType.Gets;
            return await _rpcClient.RequestAsync<PageOptionsDto, IEnumerable<MessageDto>>(opts) ?? Enumerable.Empty<MessageDto>();
        }

        public async Task<bool> AddAsync(MessageCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (!await _context.CanCreateBaccountAsync())
            //    throw new UnauthorizedAccessException();

            entity.TenantUID = _tenantResolver.GetTenantUID();
            entity.EventType = CrudActionType.Create;
            var created = await _rpcClient.RequestAsync<MessageCreateDto, MessageDto>(entity);
            if (created == null)
                return false;

            entity.MessageUID = created.MessageUID;
            return true;
        }

        public async Task<bool> UpdateAsync(MessageUpdateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //if (!await _context.CanEditBaccountAsync())
            //    throw new UnauthorizedAccessException();

            entity.TenantUID = _tenantResolver.GetTenantUID();
            entity.EventType = CrudActionType.Update;
            var updated = await _rpcClient.RequestAsync<MessageUpdateDto, BoolDto>(entity);
            return updated?.Done ?? false;
        }

        public async Task<MessageDto?> GetByIdAsync(Guid id)
        {
            //if (!await _context.CanReadBaccountAsync())
            //    throw new UnauthorizedAccessException();

            var guidDto = new GuidDto
            {
                Id = id,
                EventType = CrudActionType.Get,
                TenantUID = _tenantResolver.GetTenantUID()
            };

            return await _rpcClient.RequestAsync<GuidDto, MessageDto>(guidDto);
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

        public Task<IEnumerable<MessageDto>> GetAllAsync(PageOptionsDto opts)
        {
            throw new NotImplementedException();
        }
    }
}
