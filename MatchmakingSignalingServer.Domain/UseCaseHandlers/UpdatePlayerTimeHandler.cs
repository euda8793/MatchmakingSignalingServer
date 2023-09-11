namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

internal class UpdatePlayerTimeHandler : IUseCaseHandler<UpdatePlayerTime>
{
    private readonly IGameSessionData gameSessionData;

    public UpdatePlayerTimeHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    public async Task<UseCaseResult> Handle(UpdatePlayerTime useCase)
    {
        var gameSession = await gameSessionData
            .GameSessions
            .AsTracking()
            .Include(x => x.Clients)
            .FirstAsync(x => x.GameSessionName == useCase.GameSessionName);

        gameSession.UpdatePlayerExpirationTime(useCase.RefreshedPlayerName);

        await gameSessionData.SaveChangesAsync();
        return new UseCaseResult();
    }
}
