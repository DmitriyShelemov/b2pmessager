using DbUp;
using FluentValidation;
using chatservice.Dto;
using chatservice.Services;
using chatservice.Services.Interfaces;
using chatservice.Validators;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Net.Mime;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IValidator<ChatCreateDto>, ChatCreateDtoValidator>();
builder.Services.AddTransient<ITenantResolver, TenantResolver>();
builder.Services.AddTransient<IChatService, chatservice.Services.ChatService>();
builder.Services.AddTransient<IGenericRepository<ChatDto>, ChatRepository>();
builder.Services.AddSingleton(typeof(ISqlGenerator<>), typeof(SqlGenerator<>));

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


builder.Services.AddTransient<IDbConnection>((sp) => {
    var conn = new SqlConnection(dbConnectionString);
    var contextAccessor = sp.GetService<IHttpContextAccessor>();
    var tenantUID = contextAccessor?.HttpContext?.GetRouteValue("tenantUID");
    if (tenantUID != null)
    {
        conn.Open();
        if (tenantUID != null) { conn.SetTenantUID(tenantUID); }
    }
    return conn;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //c.IncludeXmlComments(string.Format(@"{0}\chatservice.xml", System.AppDomain.CurrentDomain.BaseDirectory));
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "chatservice",
    });

    //c.AddSecurityDefinition(
    //    JwtBearerDefaults.AuthenticationScheme,
    //    new OpenApiSecurityScheme
    //    {
    //        Name = "Authorization",
    //        Description = "JWT Authorization: Bearer {token}",
    //        In = ParameterLocation.Header,
    //        Type = SecuritySchemeType.Http,
    //        Scheme = JwtBearerDefaults.AuthenticationScheme
    //    });

    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //                {
    //                    {
    //                        new OpenApiSecurityScheme
    //                        {
    //                            Name = JwtBearerDefaults.AuthenticationScheme,
    //                            In = ParameterLocation.Header,
    //                            Reference = new OpenApiReference
    //                            {
    //                                Id = JwtBearerDefaults.AuthenticationScheme,
    //                                Type = ReferenceType.SecurityScheme
    //                            }
    //                        },
    //                        new List<string>()
    //                    }
    //                });
});

var app = builder.Build();

//app.UseHttpsRedirection();
app.UseRouting();
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature?.Error is UnauthorizedAccessException)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await Task.CompletedTask;
        }
        else if (exceptionHandlerPathFeature?.Error is System.ComponentModel.DataAnnotations.ValidationException)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(exceptionHandlerPathFeature.Error.Message);
        }
    });
});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();