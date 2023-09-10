namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

internal class CheckSignalingInfoFromHostHandler : IUseCaseHandler<CheckSignalingInfoFromHost>
{
    private readonly IGameSessionData gameSessionData;

    public CheckSignalingInfoFromHostHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    public async Task<UseCaseResult> Handle(CheckSignalingInfoFromHost useCase)
    {
        var gameSession = await gameSessionData
            .GameSessions
            .AsNoTracking()
            .Include(x => x.Clients)
            .Include(x => x.Steps)
            .FirstAsync(x => x.GameSessionName == useCase.GameSessionName);

        var signalingStep = gameSession.GetSignalingStepToPlayer(useCase.RequestingPlayerName);

        return new PlayerSignalingStepResult(useCase.RequestingPlayerName, signalingStep.InformationType, signalingStep.IceCandidate, signalingStep.SessionDescription);
    }
}
