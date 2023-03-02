using facadeservice.Dto;

namespace facadeservice.Services.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync(PageOptionsDto opts);

        Task<IEnumerable<T>> GetAllAsync(Guid parentId, PageOptionsDto opts);

        Task<bool> AddAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(Guid id);
    }
}
