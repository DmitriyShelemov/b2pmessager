using chatservice.Dto;
using chatservice.Services.Interfaces;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using System.Data;

namespace chatservice.Services
{
    public class ChatRepository : DapperRepository<ChatDto>, IGenericRepository<ChatDto>
    {
        public ChatRepository(IDbConnection connection, ISqlGenerator<ChatDto> sqlGenerator)
            : base(connection, sqlGenerator)
        {
        }

        public async Task<IEnumerable<ChatDto>> GetAllAsync(PageOptionsDto opts)
        {
            return (await SetLimit(opts.Take, opts.Skip).FindAllAsync(x => !x.Deleted)).ToArray();
        }

        public async Task<bool> AddAsync(ChatDto entity)
        {
            return await base.InsertAsync(entity);
        }

        public async Task<bool> UpdateAsync(ChatDto entity)
        {
            return await base.UpdateAsync(x => x.ChatID == entity.ChatID && !x.Deleted, entity);
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
