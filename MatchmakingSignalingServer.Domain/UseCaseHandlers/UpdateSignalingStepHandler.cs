﻿namespace MatchmakingSignalingServer.Domain.UseCaseHandlers;

internal class UpdateSignalingStepHandler : IUseCaseHandler<UpdateSignalingStep>
{
    private readonly IGameSessionData gameSessionData;

    public UpdateSignalingStepHandler(IGameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
    }

    public async Task<UseCaseResult> Handle(UpdateSignalingStep useCase)
    {
        var gameSession = await gameSessionData
            .GameSessions
            .AsTracking()
            .Include(x => x.Steps)
            .FirstAsync(x => x.GameSessionName == useCase.GameSessionName);

        gameSession.UpdateStep(useCase.RequestingPlayerName, useCase.InformationType, useCase.IceCandidate, useCase.SessionDescription);
        await gameSessionData.SaveChangesAsync();

        return new UseCaseResult();
    }
}