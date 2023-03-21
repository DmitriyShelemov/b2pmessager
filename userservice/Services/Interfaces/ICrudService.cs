using userservice.Dto;

namespace userservice.Services.Interfaces
{
    public interface ICrudService<T, R, U> where T : class where R : class
    {
        Task<IEnumerable<T>> GetAllAsync(PageOptionsDto opts);

        Task<T> GetByIdAsync(Guid id);

        Task<bool> AddAsync(R entity);

        Task<bool> UpdateAsync(U entity);

        Task<bool> DeleteAsync(Guid id);
    }
}
