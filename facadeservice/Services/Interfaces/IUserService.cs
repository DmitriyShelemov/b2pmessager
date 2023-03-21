using facadeservice.Dto;

namespace facadeservice.Services.Interfaces
{
    public interface IUserService : ICrudService<UserDto, UserCreateDto, UserUpdateDto>
    {
        Task<bool> AddAsync(UserCreateDto entity, string verificationUrl);

        Task<bool> SignupAsync(UserSignupDto entity, string verificationUrl);

        Task<bool> ActivateAsync(UserActivateDto entity);
    }
}
