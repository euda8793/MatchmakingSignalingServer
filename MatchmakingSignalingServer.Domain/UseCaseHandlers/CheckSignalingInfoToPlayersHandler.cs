namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

internal class CheckSignalingInfoToPlayersHandler : IUseCaseHandler<CheckSignalWithPlayers>
{
    private readonly IGameSessionData gameSessionData;

    public CheckSignalingInfoToPlayersHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    public async Task<UseCaseResult> Handle(CheckSignalWithPlayers useCase)
    {
        var gameSession = await gameSessionData
            .GameSessions
            .AsNoTracking()
            .Include(x => x.Clients)
            .Include(x => x.Steps)
            .FirstAsync(x => x.GameSessionName == useCase.GameSessionName);

        var signalingSteps = gameSession.GetSignalingStepsForHost();

        return new HostSignalingStepResult(
            signalingSteps.Select(
                x => new PlayerSignalingStepResult(
                    x.InformationType, 
                    x.IceCandidate, 
                    x.SessionDescription))
            .ToList());
    }
}
