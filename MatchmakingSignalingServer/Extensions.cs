
namespace MatchmakingSignalingServer.API;

public static class Extensions
{
    public static void AddUseCaseHandlers(this IServiceCollection services)
    {
        services.AddTransient<IUseCaseHandler<RetrieveAllGameSessions>>();
    }

}
