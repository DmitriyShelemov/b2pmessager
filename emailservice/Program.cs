using emailservice.Dto;
using emailservice.Services;
using emailservice.Services.Interfaces;
using emailservice.Validators;
using FluentValidation;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IValidator<VerifyEmailDto>, VerifyEmailDtoValidator>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddSingleton(typeof(ISqlGenerator<>), typeof(SqlGenerator<>));
builder.Services.AddScoped<IEventProcessor<BaseEvent<CrudActionType>>, EmailEventProcessor>();
builder.Services.AddHostedService<EmailRpc>();
builder.Services.AddSingleton<IMessageConnection, MessageConnection>();

var app = builder.Build();

app.Run();