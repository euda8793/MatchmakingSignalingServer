namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

internal class UpdateSignalingStepHandler : IUseCaseHandler<UpdateSignalingStep>
{
    private readonly IGameSessionData gameSessionData;

    public UpdateSignalingStepHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    public async Task<UseCaseResult> Handle(UpdateSignalingStep useCase)
    {
        throw new NotImplementedException();
    }
}
