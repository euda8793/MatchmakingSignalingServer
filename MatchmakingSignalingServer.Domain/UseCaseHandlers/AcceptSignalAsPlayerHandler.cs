namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

internal class AcceptSignalAsPlayerHandler : IUseCaseHandler<AcceptSignalAsPlayer>
{
    private readonly IGameSessionData gameSessionData;

    public AcceptSignalAsPlayerHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    public async Task<UseCaseResult> Handle(AcceptSignalAsPlayer useCase)
    {
        var gameSession = await gameSessionData
            .GameSessions
            .AsTracking()
            .Include(x => x.Clients)
            .Include(x => x.Steps)
            .FirstAsync(x => x.GameSessionName == useCase.GameSessionName);

        gameSession.SetHostAsTarget(useCase.RequestingPlayerName, useCase.InformationType, useCase.IceCandidate, useCase.SessionDescription);

        await gameSessionData.SaveChangesAsync();
        return new UseCaseResult();
    }
}
