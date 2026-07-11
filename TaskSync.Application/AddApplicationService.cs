using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TaskSync.Application;

public static class AddApplicationService
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
