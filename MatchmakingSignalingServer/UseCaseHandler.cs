namespace MatchmakingSignalingServer.API;

internal class UseCaseHandlerResolver : IResolveUseCaseHandler
{
    private readonly IServiceProvider serviceProvider;

    public UseCaseHandlerResolver(IServiceProvider serviceProvider) 
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task<UseCaseResult> ResolveAndHandleUseCase<T>(T useCase) where T : UseCase
    {
        var maybeHandler = serviceProvider.GetService(typeof(IUseCaseHandler<T>));
        if (!(maybeHandler is IUseCaseHandler<T> handler)) throw new UseCaseHandlerMissingException(typeof(T));
        
        return await handler.Handle(useCase);
    }
}
