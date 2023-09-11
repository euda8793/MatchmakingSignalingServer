namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

internal class UpdateSignalingStepFromHostHandler : IUseCaseHandler<UpdateSignalingStepFromHost>
{
    private readonly IGameSessionData gameSessionData;

    public UpdateSignalingStepFromHostHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    public async Task<UseCaseResult> Handle(UpdateSignalingStepFromHost useCase)
    {
        var gameSession = await gameSessionData
            .GameSessions
            .AsTracking()
            .Include(x => x.Steps)
            .Include(x => x.Clients)
            .FirstAsync(x => x.GameSessionName == useCase.GameSessionName);

        gameSession.UpdateStepFromHost(useCase.TargetPlayerName, useCase.InformationType, useCase.IceCandidate, useCase.SessionDescription);
        await gameSessionData.SaveChangesAsync();

        return new UseCaseResult();
    }
}
