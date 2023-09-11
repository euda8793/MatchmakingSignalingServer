namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

internal class NotifyPlayerConnectedHandler : IUseCaseHandler<NotifyPlayerConnected>
{
    private readonly IGameSessionData gameSessionData;

    public NotifyPlayerConnectedHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    public async Task<UseCaseResult> Handle(NotifyPlayerConnected useCase)
    {
        var gameSession = await gameSessionData
            .GameSessions
            .AsTracking()
            .Include(x => x.Steps)
            .Include(x => x.Clients)
            .FirstAsync(x => x.GameSessionName == useCase.GameSessionName);

        gameSession.UpdatePlayerToConnected(useCase.ConnectedPlayerName);

        await gameSessionData.SaveChangesAsync();
        return new UseCaseResult();
    }
}
