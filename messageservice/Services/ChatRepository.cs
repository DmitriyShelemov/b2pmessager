using messageservice.Dto;
using messageservice.Services.Interfaces;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using System.Data;

namespace messageservice.Services
{
    public class ChatRepository : DapperRepository<ChatDto>, IGenericRepository<ChatDto>
    {
        public ChatRepository(IDbConnection connection, ISqlGenerator<ChatDto> sqlGenerator)
            : base(connection, sqlGenerator)
        {
        }

        public async Task<IEnumerable<ChatDto>> GetAllAsync(Guid parentId, PageOptionsDto opts)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddAsync(ChatDto entity)
        {
            return await base.InsertAsync(entity);
        }

        public async Task<bool> UpdateAsync(ChatDto entity)
        {
            return await base.UpdateAsync(x => x.ChatUID == entity.ChatUID, entity);
        }

        public async Task<ChatDto> GetByIdAsync(Guid id)
        {
            return await FindAsync(x => x.ChatUID == id && !x.Deleted);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }

}
