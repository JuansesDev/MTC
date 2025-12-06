using Microsoft.Extensions.DependencyInjection;

namespace MyCleanApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services here (e.g. MediatR, Validators)
        return services;
    }
}
