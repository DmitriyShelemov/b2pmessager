using FluentValidation;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;
using System.Text.Json;
using userservice.Dto;
using userservice.Services.Interfaces;

namespace userservice.Services
{
    public class UserEventProcessor : IEventProcessor<BaseEvent<CrudActionType>>
    {
        private readonly IUserService _service;
        private readonly IUserBuilder _builder;
        private readonly IValidator<UserCreateDto> _validator;
        private readonly IValidator<GuidDto> _guidValidator;
        private readonly IValidator<PageOptionsDto> _pagingValidator;
        private readonly IMessagePublisher<UserDto> _publisher;

        public UserEventProcessor(
            IUserService service,
            IMessagePublisher<UserDto> publisher,
            IValidator<UserCreateDto> validator,
            IValidator<GuidDto> guidValidator,
            IValidator<PageOptionsDto> pagingValidator,
            IUserBuilder builder)
        {
            _service = service;
            _publisher = publisher;
            _validator = validator;
            _guidValidator = guidValidator;
            _pagingValidator = pagingValidator;
            _builder = builder;
        }

        public async Task<string?> ProcessEvent(BaseEvent<CrudActionType>? message, string src)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            switch (message.EventType)
            {
                case CrudActionType.Get:
                    return await CallService<GuidDto>(src, async (m) => {

                        var result = await _guidValidator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        return await _service.GetByIdAsync(m.Id); 
                    });
                case CrudActionType.Gets:
                    return await CallService<PageOptionsDto>(src, async (m) => {

                        var result = await _pagingValidator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        return await _service.GetAllAsync(m); 
                    });
                case CrudActionType.Create:
                case CrudActionType.Signup:
                    return await CallService(src, (Func<UserCreateDto, Task<object?>>)(async (m) =>
                    {

                        var result = await _validator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        var existing = await _service.FindUserByEmail(m.Email);
                        if (existing != null && existing.Activated)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        return (existing == null) ? await BuildNewUserAsync(m) : await BuildExistingUserAsync(existing, m.VerificationUrl);
                    }));
                case CrudActionType.Activate:
                    return await CallService<ActivateDto>(src, async (m) => {

                        var result = await _guidValidator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        var (done, user) = await _service.ActivateAsync(m.Id, m.VerificationKey);
                        if (done)
                        {
                            var created = new UserDto
                            {
                                Name = user.Name,
                                UserUID = user.UserUID,
                                Activated = true
                            };
                            _publisher.Publish(created);
                        }
                        else
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        return new BoolDto { Done = done };
                    });
                case CrudActionType.Update:
                    return await CallService<UserCreateDto>(src, async (m) => {

                        var result = await _validator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        return new BoolDto { Done = await _service.UpdateAsync(m) };
                    });
                case CrudActionType.Delete:
                    return await CallService<GuidDto>(src, async (m) => {

                        var result = await _guidValidator.ValidateAsync(m);
                        if (!result.IsValid)
                        {
                            return null;
                            //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
                        }

                        var done = await _service.DeleteAsync(m.Id);
                        if (done)
                        {
                            _publisher.Publish(new UserDto
                            {
                                UserUID = m.Id,
                                Deleted = done
                            });
                        }
                        else
                        {

                        }

                        return new BoolDto { Done = done };
                    });
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task<UserDto?> BuildNewUserAsync(UserCreateDto m)
        {
            if (!await _builder.CreateAsync(m.Email, m.Password, $"{m.FirstName} {m.LastName}"))
            {
                return null;
                //return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToArray());
            }

            m.VerificationKey = Guid.NewGuid().ToString("N");
            m.Activated = false;
            var done = await _service.AddAsync(m);
            if (done)
            {
                var created = new UserDto
                {
                    Name = m.Name,
                    UserUID = m.UserUID
                };
                _publisher.Publish(created);

                await _builder.VerifyEmailAsync(m.Email, string.Format(m.VerificationUrl, m.UserUID.ToString("D"), m.VerificationKey));

                return created;
            }
            else
            {
                return null;
            }
        }

        private async Task<UserDto?> BuildExistingUserAsync(UserDto m, string verificationUrl)
        {
            var verificationKey = Guid.NewGuid().ToString("N");
            var done = await _service.UpdateAsync(new UserCreateDto { VerificationKey = verificationKey });
            if (done)
            {
                var created = new UserDto
                {
                    Name = m.Name,
                    UserUID = m.UserUID
                };
                _publisher.Publish(created);

                await _builder.VerifyEmailAsync(m.Email, string.Format(verificationUrl, m.UserUID.ToString("D"), verificationKey));

                return created;
            }
            else
            {
                return null;
            }
        }

        private async Task<string?> CallService<T>(string src, Func<T, Task<object?>> act) where T : IBaseEvent<CrudActionType>
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));
            if (act == null)
                throw new ArgumentNullException(nameof(act));

            var getRequest = JsonSerializer.Deserialize<T>(src);
            if (getRequest == null)
                throw new ArgumentNullException(nameof(getRequest));

            var getResponse = await act(getRequest);
            return getResponse != null ? JsonSerializer.Serialize(getResponse) : null;
        }
    }
}
