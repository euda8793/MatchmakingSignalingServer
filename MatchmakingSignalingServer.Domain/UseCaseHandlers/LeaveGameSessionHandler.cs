namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

internal class LeaveGameSessionHandler : IUseCaseHandler<LeaveGameSession>
{
    private readonly IGameSessionData gameSessionData;

    public LeaveGameSessionHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    public async Task<UseCaseResult> Handle(LeaveGameSession useCase)
    {
        var gameSession = await gameSessionData
            .GameSessions
            .AsTracking()
            .Include(x => x.Clients)
            .Include(x => x.Steps)
            .FirstAsync(x => x.GameSessionName == useCase.GameSessionName);

        if (gameSession.IsPlayerHost(useCase.LeavingPlayerOrHostName))
        {
            gameSessionData.GameSessions.Remove(gameSession);
        }
        else
        {
            gameSession.RemovePlayer(useCase.LeavingPlayerOrHostName);
        }

        await gameSessionData.SaveChangesAsync();
        return new UseCaseResult();
    }
}
