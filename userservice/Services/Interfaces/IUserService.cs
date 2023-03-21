using userservice.Dto;

namespace userservice.Services.Interfaces
{
    public interface IUserService : ICrudService<UserDto, UserCreateDto, UserCreateDto>
    {
        Task<UserDto> FindUserByEmail(string email);

        Task<(bool, UserDto?)> ActivateAsync(Guid id, string? verificationKey);
    }
}
