using facadeservice.Dto;
using facadeservice.Services;
using facadeservice.Services.Interfaces;
using facadeservice.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IValidator<TenantCreateDto>, TenantCreateDtoValidator>();
builder.Services.AddTransient<IValidator<ChatCreateDto>, ChatCreateDtoValidator>();
builder.Services.AddTransient<IValidator<MessageCreateDto>, MessageCreateDtoValidator>();
builder.Services.AddTransient<IValidator<MessageUpdateDto>, MessageUpdateDtoValidator>();
builder.Services.AddTransient<ITenantResolver, TenantResolver>();
builder.Services.AddTransient<ITenantService, TenantService>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddSingleton<IRpcClient<TenantDto>, TenantRpcClient>();
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //c.IncludeXmlComments(string.Format(@"{0}\messageservice.xml", System.AppDomain.CurrentDomain.BaseDirectory));
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "b2pmessager",
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