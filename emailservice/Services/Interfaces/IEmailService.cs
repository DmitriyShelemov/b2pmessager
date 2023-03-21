using emailservice.Dto;

namespace emailservice.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendVerificationAsync(VerifyEmailDto dto);
    }
}
