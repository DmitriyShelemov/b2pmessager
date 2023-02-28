﻿using tenantservice.Dto;

namespace tenantservice.Services.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync(PageOptionsDto opts);

        Task<bool> AddAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(Guid id);
    }
}
