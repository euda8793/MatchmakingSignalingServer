﻿namespace MatchmakingSignalingServer.Domain;

public record UseCaseResult;

public record GameSessionsResult(List<string> GameSessions) : UseCaseResult;

public record PlayerSignalingStepResult(InformationType InformationType, IceCandidate? IceCandidate, SessionDescription? SessionDescription) : UseCaseResult;

public record HostSignalingStepResult(List<PlayerSignalingStepResult> SignalingStepResults) : UseCaseResult;
