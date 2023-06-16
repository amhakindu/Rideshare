using Rideshare.Persistence;
using Rideshare.Application;
using Microsoft.OpenApi.Models;
using Serilog;
using Rideshare.WebApi.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Rideshare.WebApi;
using Rideshare.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.ConfigureApplicationService();
builder.Services.ConfigurePersistenceService(builder.Configuration);
builder.Services.AddTransient<ExceptionHandler>();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.ConfigureInfrastructureService(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Host.UseSerilog();

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
app.UseMiddleware<ExceptionHandler>();

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
            Type = SecuritySchemeType.Http,
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