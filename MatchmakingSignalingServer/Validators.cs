namespace MatchmakingSignalingServer.API.Validators;

public class HostGameSessionRequestValidator : AbstractValidator<HostGameSessionRequest>    
{
    public HostGameSessionRequestValidator()
    {
        RuleFor(request => request.GameSessionName)
            .MinimumLength(GameSessionName.MinGameSessionNameChars)
            .MaximumLength(GameSessionName.MaxGameSessionNameChars)
            .WithMessage(x => $"{nameof(x.GameSessionName)} did not meet length requirement.");
    }
}
