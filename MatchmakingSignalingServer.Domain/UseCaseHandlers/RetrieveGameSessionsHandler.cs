namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

public class RetrieveGameSessionsHandler : IUseCaseHandler<RetrieveGameSessions>
{
    private readonly IGameSessionData gameSessionData;

    public RetrieveGameSessionsHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    public async Task<UseCaseResult> Handle(RetrieveGameSessions useCase)
    {
        var gameSessions = await gameSessionData
            .GameSessions
            .AsNoTracking()
            .Take(useCase.NumberOfGameSessions.Count)
            .Select(x => x.GameSessionName.Name)
            .ToListAsync();

        return new GameSessionsResult(gameSessions ?? new List<string>());
    }
}
