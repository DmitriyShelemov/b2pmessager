using DbUp;
using FluentValidation;
using chatservice.Dto;
using chatservice.Services;
using chatservice.Services.Interfaces;
using chatservice.Validators;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using queuemessagelibrary.MessageBus.Interfaces;
using queuemessagelibrary.MessageBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IValidator<GuidDto>, GuidDtoValidator>();
builder.Services.AddTransient<IValidator<PageOptionsDto>, PageOptionsDtoValidator>();
builder.Services.AddTransient<IValidator<ChatCreateDto>, ChatCreateDtoValidator>();
builder.Services.AddTransient<ITenantResolver, TenantResolver>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IGenericRepository<ChatDto>, ChatRepository>();
builder.Services.AddSingleton(typeof(ISqlGenerator<>), typeof(SqlGenerator<>));
builder.Services.AddScoped<IEventProcessor<BaseEvent<CrudActionType>>, ChatEventProcessor>();
builder.Services.AddHostedService<ChatRpc>();
builder.Services.AddHostedService<TenantSubscriber>();
builder.Services.AddSingleton<IMessagePublisher<ChatDto>, ChatPublisher>();
builder.Services.AddSingleton<IMessageConnection, MessageConnection>();

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