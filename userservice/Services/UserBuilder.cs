using userservice.Services.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using userservice.Dto;

namespace userservice.Services
{
    public class UserBuilder : IUserBuilder
    {
        private readonly FirebaseApp _firebaseApp;
        private readonly IRpcClient<VerifyEmailDto> _rpcEmailClient;

        public UserBuilder(FirebaseApp firebaseApp, 
            IRpcClient<VerifyEmailDto> rpcEmailClient)
        {
            _firebaseApp = firebaseApp;
            _rpcEmailClient = rpcEmailClient;
        }

        public async Task<bool> CreateAsync(string email, string password, string name)
        {
            var client = CreateClient();

            try
            {
                var user = await client.CreateUserAsync(new UserRecordArgs
                {
                    Email = email,
                    Password = password,
                    DisplayName = name,
                    EmailVerified = false,
                    Disabled = true
                });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task ResetPasswordAsync(string email)
        {
            var client = CreateClient();
            var link = await client.GeneratePasswordResetLinkAsync(email);
            //TODO
        }

        public async Task VerifyEmailAsync(string email, string verificationUrl)
        {
            var client = CreateClient();
            var link = await client.GenerateEmailVerificationLinkAsync(email, new ActionCodeSettings
            {
                Url = verificationUrl
            });

            await _rpcEmailClient.RequestAsync<VerifyEmailDto, BoolDto>(new VerifyEmailDto
            {
                To = email,
                Link = link,
                EventType = CrudActionType.Verify
            });
        }

        private FirebaseAuth CreateClient()
        {
            return FirebaseAuth.GetAuth(_firebaseApp);
        }
    }
}
