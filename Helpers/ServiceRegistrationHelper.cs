using System.Reflection;

namespace DodjelaStanovaZG.Helpers;

public static class ServiceRegistrationHelper
{
    public static IServiceCollection RegisterBySuffix(this IServiceCollection services, string suffix, params Assembly[]? assemblies)
    {
        if (assemblies is null || assemblies.Length == 0)
        {
            assemblies = [Assembly.GetExecutingAssembly()];
        }

        var types = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.Name.EndsWith(suffix));

        foreach (var impl in types)
        {
            var interfaces = impl.GetInterfaces()
                .Where(i => i.Name.EndsWith(suffix));

            var enumerable = interfaces as Type[] ?? interfaces.ToArray();
            if (enumerable.Length != 0)
            {
                foreach (var iface in enumerable)
                {
                    services.AddScoped(iface, impl);
                }
            }
            else
            {
                services.AddScoped(impl);
            }
        }

        return services;
    }
}