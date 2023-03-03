using facadeservice.Dto;
using facadeservice.Services.Interfaces;

namespace facadeservice.Services
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IRpcClient<T> _rpcClient;

        public GenericRepository(IRpcClient<T> rpcClient)
        {
            _rpcClient = rpcClient;
        }

        public Task<bool> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync(PageOptionsDto opts)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync(Guid parentId, PageOptionsDto opts)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
