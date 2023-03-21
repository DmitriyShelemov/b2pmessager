namespace userservice.Services.Interfaces
{
    public interface IUserBuilder
    {
        Task<bool> CreateAsync(string email, string password, string name);

        Task ResetPasswordAsync(string email);

        Task VerifyEmailAsync(string email, string verificationUrl);
    }
}
