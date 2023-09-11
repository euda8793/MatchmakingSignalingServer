namespace MatchmakingSignalingServer.Domain;

public interface IUseCaseHandler<T> where T : UseCase
{
    public Task<UseCaseResult> Handle(T useCase);
}
