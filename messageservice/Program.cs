using DbUp;
using FluentValidation;
using messageservice.Dto;
using messageservice.Services;
using messageservice.Services.Interfaces;
using messageservice.Validators;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using Microsoft.Data.SqlClient;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;
using System.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IValidator<GuidDto>, GuidDtoValidator>();
builder.Services.AddTransient<IValidator<PageOptionsDto>, PageOptionsDtoValidator>();
builder.Services.AddTransient<IValidator<MessageCreateDto>, MessageCreateDtoValidator>();
builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddTransient<IGenericRepository<MessageDto>, MessageRepository>();
builder.Services.AddTransient<IGenericRepository<TenantDto>, TenantRepository>();
builder.Services.AddTransient<IGenericRepository<ChatDto>, ChatRepository>();
builder.Services.AddScoped<ITenantResolver, TenantResolver>();
builder.Services.AddScoped<IEventProcessor<BaseEvent<CrudActionType>>, MessageEventProcessor>();
builder.Services.AddScoped<IEventProcessor<TenantDto>, TenantPublishingProcessor>();
builder.Services.AddScoped<IEventProcessor<ChatDto>, ChatPublishingProcessor>();
builder.Services.AddSingleton(typeof(ISqlGenerator<>), typeof(SqlGenerator<>));
builder.Services.AddSingleton<IMessageConnection, MessageConnection>();
builder.Services.AddHostedService<TenantSubscriber>();
builder.Services.AddHostedService<ChatSubscriber>();
builder.Services.AddHostedService<ChatMessageRpc>();

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