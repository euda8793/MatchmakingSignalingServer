namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

internal class AcceptSignalAsHostHandler : IUseCaseHandler<AcceptSignalAsHost>
{
    private readonly IGameSessionData gameSessionData;

    public AcceptSignalAsHostHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    public async Task<UseCaseResult> Handle(AcceptSignalAsHost useCase)
    {
        var gameSession = await gameSessionData
            .GameSessions
            .AsTracking()
            .Include(x => x.Clients)
            .Include(x => x.Steps)
            .FirstAsync(x => x.GameSessionName == useCase.GameSessionName);

        gameSession.SetConnectedPlayerAsTarget(useCase.ConnectedPlayerName, useCase.InformationType, useCase.IceCandidate, useCase.SessionDescription);
        await gameSessionData.SaveChangesAsync();
        return new UseCaseResult();
    }
}
