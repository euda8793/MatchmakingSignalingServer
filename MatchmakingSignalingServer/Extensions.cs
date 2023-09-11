namespace MatchmakingSignalingServer.API.Extensions;

public static class Extensions
{
    public static void AddUseCaseHandlers(this IServiceCollection services)
    {
        services.AddScoped<IResolveUseCaseHandler, UseCaseHandlerResolver>();

        var handlerType = typeof(IUseCaseHandler<>);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(p => !p.IsInterface && (p.GetInterfaces()?.Select(x => x.Name).ToList().Contains(handlerType.Name) ?? false));

        foreach (var type in types)
        {
            var interf = type.GetInterface(handlerType.Name);
            Console.WriteLine($"mapping {nameof(interf)} to {nameof(type)}");
            services.AddScoped(interf, type);
        }
    }

}
