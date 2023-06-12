using MediatR;
using System.Net;
using Rideshare.Persistence;
using Rideshare.Application;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics;
using Rideshare.Application.Exceptions;
using Rideshare.Application.Responses;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.ConfigureApplicationService();
builder.Services.ConfigurePersistenceService(builder.Configuration);
builder.Host.UseSerilog();

builder.Services.AddHttpContextAccessor();
AddSwaggerDoc(builder.Services);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o =>
{
    o.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rideshare.WebApi v1"));
app.UseHttpsRedirection();
app.UseAuthorization();

// Use Serilog for logging
app.UseSerilogRequestLogging();
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        // Get the exception that occurred
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        // Log the exception
        Log.Error(exception, "An unhandled exception occurred: ");

        // Return a proper response to client
        if(exception is ValidationException){
            context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
            await context.Response.WriteAsJsonAsync<BaseResponse<Unit>>(
                new BaseResponse<Unit>{
                    Success=false,
                    Message="Request Data Validation Failed",
                    Errors=new List<string>{exception.Message}
                }
            );
            return;
        }else if(exception is NotFoundException){
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync<BaseResponse<Unit>>(
                new BaseResponse<Unit>{
                    Success=false,
                    Message="Resource Not Found",
                    Errors=new List<string>{exception.Message}
                }
            );
            return;
        }else if(exception is InternalServerErrorException){
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync<BaseResponse<Unit>>(
                new BaseResponse<Unit>{
                    Success=false,
                    Message="Failed to Process Request",
                    Errors=new List<string>{exception.Message}
                }
            );
            return;
        }
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsJsonAsync<BaseResponse<Unit>>(
            new BaseResponse<Unit>{
                Success=false,
                Message="Failed To Process Request",
                Errors=new List<string>{exception.Message}
            }
        );
    });
});

app.MapControllers();

app.Run();

void AddSwaggerDoc(IServiceCollection services)
{
    services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Rideshare API",
            Version = "v1",
            Description = "Rideshare API Services",
            Contact = new OpenApiContact
            {
                Name = "Rideshare Backend Team"
            },
        });
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme."
        });
        
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });
}