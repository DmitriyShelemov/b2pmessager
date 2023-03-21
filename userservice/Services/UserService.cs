using userservice.Dto;
using userservice.Services.Interfaces;

namespace userservice.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(
            IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDto> FindUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            return await _repository.FindUserByEmail(email);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(PageOptionsDto opts)
        {
            return await _repository.GetAllAsync(opts);
        }

        public async Task<bool> AddAsync(UserCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.UserUID = Guid.NewGuid();
            return await _repository.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(UserCreateDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var old = await _repository.GetByIdAsync(entity.UserUID);
            if (old != null)
            {
                if (entity.FirstName != null) old.FirstName = entity.FirstName;
                if (entity.LastName != null) old.LastName = entity.LastName;
                if (entity.VerificationKey != null) old.VerificationKey = entity.VerificationKey;

                return await _repository.UpdateAsync(old);
            }

            return false;
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<(bool, UserDto?)> ActivateAsync(Guid id, string? verificationKey)
        {
            if (verificationKey == null)
            {
                throw new ArgumentNullException(nameof(verificationKey));
            }

            var old = await _repository.GetByIdAsync(id);
            if (old != null && verificationKey == old.VerificationKey && !old.Activated)
            {
                old.Activated = true;
                var result = await _repository.UpdateAsync(old);
                return result ? (true, old) : (false, null);
            }

            return (false, null);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var old = await _repository.GetByIdAsync(id);
            if (old != null)
            {
                old.Deleted = true;
                return await _repository.UpdateAsync(old);
            }

            return false;
        }
    }
}
