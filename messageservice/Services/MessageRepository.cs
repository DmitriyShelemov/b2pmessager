using messageservice.Dto;
using messageservice.Services.Interfaces;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using System.Data;

namespace messageservice.Services
{
    public class MessageRepository : DapperRepository<MessageDto>, IGenericRepository<MessageDto>
    {
        public MessageRepository(IDbConnection connection, ISqlGenerator<MessageDto> sqlGenerator)
            : base(connection, sqlGenerator)
        {
        }

        public async Task<IEnumerable<MessageDto>> GetAllAsync(Guid parentId, PageOptionsDto opts)
        {
            return (await FindAllAsync(x => !x.Deleted && x.ChatUID == parentId)).ToArray();
        }

        public Task<IEnumerable<MessageDto>> GetAllAsync(int teamId, PageOptionsDto opts)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddAsync(MessageDto entity)
        {
            return await base.InsertAsync(entity);
        }

        public async Task<bool> UpdateAsync(MessageDto entity)
        {
            return await base.UpdateAsync(x => x.MessageID == entity.MessageID && !x.Deleted, entity);
        }

        public async Task<MessageDto> GetByIdAsync(Guid id)
        {
            return await FindAsync(x => x.MessageUID == id && !x.Deleted);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }

}
