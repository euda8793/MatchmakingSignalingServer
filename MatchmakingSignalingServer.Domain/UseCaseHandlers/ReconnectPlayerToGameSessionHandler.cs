namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

internal class ReconnectPlayerToGameSessionHandler : IUseCaseHandler<ReconnectPlayerToGameSession>
{
    private readonly IGameSessionData gameSessionData;

    public ReconnectPlayerToGameSessionHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    public async Task<UseCaseResult> Handle(ReconnectPlayerToGameSession useCase)
    {
        var gameSession = await gameSessionData
            .GameSessions
            .AsTracking()
            .Include(x => x.Steps)
            .Include(x => x.Clients)
            .FirstAsync(x => x.GameSessionName == useCase.GameSessionName);

        gameSession.ReconnectPlayer(useCase.ReconnectingPlayerName);

        await gameSessionData.SaveChangesAsync();
        return new UseCaseResult();
    }
}
