using Microsoft.Extensions.DependencyInjection;

namespace Zylo.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(ApplicationReference.Assembly);
        });

        return services;
    }
}
