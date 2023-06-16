using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rideshare.Application.Contracts.Services;
using Rideshare.Infrastructure.Services;

namespace Rideshare.Infrastructure;

public  static class InfrastructureServiceRegistration
{
     public static IServiceCollection ConfigureInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {

    services.AddSingleton<ISmsSender>(new TwilioSmsSender(
        configuration["Twilio:AccountSid"],
        configuration["Twilio:AuthToken"],
        configuration["Twilio:FromPhoneNumber"]
    ));
    return services;
    }

}
