using MessageService.WebApi.Dto;

namespace MessageService.WebApi.Services.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync(PageOptionsDto opts);

        Task<IEnumerable<T>> GetAllAsync(int parentId, PageOptionsDto opts);

        Task<bool> AddAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(Guid id);
    }
}
