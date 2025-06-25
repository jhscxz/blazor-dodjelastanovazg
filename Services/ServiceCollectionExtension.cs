using System.Reflection;
using DodjelaStanovaZG.Helpers;

namespace DodjelaStanovaZG.Services;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.RegisterBySuffix("Service", assembly)
            .RegisterBySuffix("Factory", assembly)
            .RegisterBySuffix("Handler", assembly)
            .RegisterBySuffix("UnitOfWork", assembly)
            .RegisterBySuffix("Repository", assembly);
    }
}