namespace MatchmakingSignalingServer.Domain;

public interface IUseCaseHandler<T>
{
    public Task<UseCaseResult> Handle(T useCase);
}
