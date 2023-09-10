namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

internal class HostGameSessionHandler : IUseCaseHandler<HostGameSession>
{
    private readonly IGameSessionData gameSessionData;

    public HostGameSessionHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    public async Task<UseCaseResult> Handle(HostGameSession useCase)
    {
        var gameSessionWithThatNameExists = await gameSessionData
            .GameSessions
            .AsNoTracking()
            .AnyAsync(x => x.GameSessionName == useCase.GameSessionName);

        if (gameSessionWithThatNameExists)
        {
            throw new ArgumentException($"A Game Session already exists with the name {useCase.GameSessionName.Name}", nameof(GameSession.GameSessionName));
        }

        var newGameSession = new GameSession(useCase.GameSessionName, useCase.HostPlayerName);
        await gameSessionData.GameSessions.AddAsync(newGameSession);
        await gameSessionData.SaveAsync();

        return new UseCaseResult();
    }
}
