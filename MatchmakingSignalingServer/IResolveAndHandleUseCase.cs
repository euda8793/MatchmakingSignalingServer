namespace MatchmakingSignalingServer.API
{
    public interface IResolveUseCaseHandler
    {
        public Task<UseCaseResult> ResolveAndHandleUseCase<T>(T useCase) where T : UseCase; 
    }
}
