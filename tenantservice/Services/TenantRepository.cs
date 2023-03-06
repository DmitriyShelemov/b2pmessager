using tenantservice.Dto;
using tenantservice.Services.Interfaces;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using System.Data;

namespace tenantservice.Services
{
    public class TenantRepository : DapperRepository<TenantDto>, IGenericRepository<TenantDto>
    {
        public TenantRepository(IDbConnection connection, ISqlGenerator<TenantDto> sqlGenerator)
            : base(connection, sqlGenerator)
        {
        }

        public async Task<IEnumerable<TenantDto>> GetAllAsync(PageOptionsDto opts)
        {
            return (await SetLimit(opts.Take, opts.Skip).FindAllAsync(x => !x.Deleted)).ToArray();
        }

        public async Task<bool> AddAsync(TenantDto entity)
        {
            return await base.InsertAsync(entity);
        }

        public async Task<bool> UpdateAsync(TenantDto entity)
        {
            return await base.UpdateAsync(x => x.TenantID == entity.TenantID && !x.Deleted, entity);
        }

        public async Task<TenantDto> GetByIdAsync(Guid id)
        {
            return await FindAsync(x => x.TenantUID == id && !x.Deleted);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }

}
