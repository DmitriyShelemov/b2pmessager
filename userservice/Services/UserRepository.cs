using userservice.Dto;
using userservice.Services.Interfaces;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using System.Data;

namespace userservice.Services
{
    public class UserRepository : DapperRepository<UserDto>, IUserRepository
    {
        public UserRepository(IDbConnection connection, ISqlGenerator<UserDto> sqlGenerator)
            : base(connection, sqlGenerator)
        {
        }

        public async Task<UserDto?> FindUserByEmail(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return await FindAsync(x => x.Email == email && !x.Deleted);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(PageOptionsDto opts)
        {
            return (await SetLimit(opts.Take, opts.Skip).FindAllAsync(x => !x.Deleted)).ToArray();
        }

        public async Task<bool> AddAsync(UserDto entity)
        {
            return await base.InsertAsync(entity);
        }

        public async Task<bool> UpdateAsync(UserDto entity)
        {
            return await base.UpdateAsync(x => x.UserID == entity.UserID && !x.Deleted, entity);
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            return await FindAsync(x => x.UserUID == id && !x.Deleted);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }

}
