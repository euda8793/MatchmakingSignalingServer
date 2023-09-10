namespace MatchmakingSignalingServer.Domain;

public record UseCaseResult;

public record HostGameSessionResult : UseCaseResult;

public record GameSessionsResult(List<string> gameSessionNames) : UseCaseResult;

public record PlayerSignalingStepResult(PlayerName PlayerName, InformationType InformationType, IceCandidate? IceCandidate, SessionDescription? SessionDescription) : UseCaseResult;

public record HostSignalingStepResult(List<PlayerSignalingStepResult> SignalingStepResults) : UseCaseResult;
