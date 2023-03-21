using DbUp;
using FirebaseAdmin;
using FluentValidation;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using Microsoft.Data.SqlClient;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;
using System.Data;
using System.Reflection;
using userservice.Dto;
using userservice.Services;
using userservice.Services.Interfaces;
using userservice.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IValidator<GuidDto>, GuidDtoValidator>();
builder.Services.AddTransient<IValidator<PageOptionsDto>, PageOptionsDtoValidator>();
builder.Services.AddTransient<IValidator<UserCreateDto>, UserCreateDtoValidator>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserBuilder, UserBuilder>();
builder.Services.AddSingleton(typeof(ISqlGenerator<>), typeof(SqlGenerator<>));
builder.Services.AddScoped<IEventProcessor<BaseEvent<CrudActionType>>, UserEventProcessor>();
builder.Services.AddSingleton<IRpcClient<VerifyEmailDto>, EmailRpcClient>();
builder.Services.AddHostedService<UserRpc>();
builder.Services.AddSingleton<IMessagePublisher<UserDto>, UserPublisher>();
builder.Services.AddSingleton<IMessageConnection, MessageConnection>();
builder.Services.AddSingleton(FirebaseApp.Create());

string dbConnectionString = builder.Configuration.GetConnectionString("SqlConnection");
EnsureDatabase.For.SqlDatabase(dbConnectionString);
var upgrader =
     DeployChanges.To
          .SqlDatabase(dbConnectionString)
          .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
          .WithTransactionPerScript()
          .LogToConsole()
          .Build();

var result = upgrader.PerformUpgrade();
if (!result.Successful)
{
    Console.WriteLine(result.Error.ToString());
}

builder.Services.AddScoped<IDbConnection>((sp) => new SqlConnection(dbConnectionString));

var app = builder.Build();

app.Run();