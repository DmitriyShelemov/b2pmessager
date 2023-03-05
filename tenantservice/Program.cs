using DbUp;
using FluentValidation;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using Microsoft.Data.SqlClient;
using queuemessagelibrary.MessageBus;
using queuemessagelibrary.MessageBus.Interfaces;
using System.Data;
using System.Reflection;
using tenantservice.Dto;
using tenantservice.Services;
using tenantservice.Services.Interfaces;
using tenantservice.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IValidator<GuidDto>, GuidDtoValidator>();
builder.Services.AddTransient<IValidator<PageOptionsDto>, PageOptionsDtoValidator>();
builder.Services.AddTransient<IValidator<TenantCreateDto>, TenantCreateDtoValidator>();
builder.Services.AddTransient<ITenantService, TenantService>();
builder.Services.AddTransient<IGenericRepository<TenantDto>, TenantRepository>();
builder.Services.AddSingleton(typeof(ISqlGenerator<>), typeof(SqlGenerator<>));
builder.Services.AddScoped<IEventProcessor<BaseEvent<CrudActionType>>, TenantEventProcessor>();
builder.Services.AddHostedService<TenantRpc>();
builder.Services.AddSingleton<IMessagePublisher<TenantDto>, TenantPublisher>();
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