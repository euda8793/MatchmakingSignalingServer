namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

internal class HostGameSessionHandler : IUseCaseHandler<HostGameSession>
{
    private readonly IGameSessionData gameSessionData;

    public HostGameSessionHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    private async Task RemoveExpiredGameSessionsIfExpired()
    {
        var expiredSessions = await gameSessionData
            .GameSessions
            .AsNoTracking()
            .Include(x => x.Clients)
            .Where(x => x.Clients.First(x => x.ConnectionState == ConnectionState.IS_HOST).Expiration > DateTime.UtcNow)
            .ToListAsync();

        if (expiredSessions.Any())
        {
            foreach (var session in expiredSessions) gameSessionData.GameSessions.Remove(session);
        }
    }

    public async Task<UseCaseResult> Handle(HostGameSession useCase)
    {
        await RemoveExpiredGameSessionsIfExpired();

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
        await gameSessionData.SaveChangesAsync();

        return new UseCaseResult();
    }
}
