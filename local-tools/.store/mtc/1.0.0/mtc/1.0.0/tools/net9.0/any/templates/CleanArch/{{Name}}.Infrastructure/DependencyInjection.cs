using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using {{Name}}.Application.Common.Interfaces;
using {{Name}}.Infrastructure.Persistence;

namespace {{Name}}.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("{{Name}}Db"));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
