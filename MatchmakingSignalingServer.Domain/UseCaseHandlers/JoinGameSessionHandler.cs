namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

internal class JoinGameSessionHandler : IUseCaseHandler<JoinGameSession>
{
    private readonly IGameSessionData gameSessionData;

    public JoinGameSessionHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    public async Task<UseCaseResult> Handle(JoinGameSession useCase)
    {
        var gameSession = await gameSessionData
            .GameSessions
            .AsTracking()
            .Include(x => x.Clients)
            .Include(x => x.Steps)
            .FirstAsync(x => x.GameSessionName == useCase.GameSessionName);

        gameSession.AddPlayerToGameSession(useCase.JoiningPlayerName);

        await gameSessionData.SaveAsync();
        return new UseCaseResult();
    }
}
