using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TaskSync.Application.Common.Behaviors;

namespace TaskSync.Application;

public static class AddApplicationService
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
