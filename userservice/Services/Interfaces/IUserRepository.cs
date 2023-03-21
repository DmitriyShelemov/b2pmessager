using userservice.Dto;

namespace userservice.Services.Interfaces
{
    public interface IUserRepository : IGenericRepository<UserDto>
    {
        Task<UserDto?> FindUserByEmail(string email);
    }
}
