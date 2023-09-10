namespace MatchmakingSignalingServer.Domain.UseCases;

public record HostGameSession(GameSessionName GameSessionName, PlayerName HostPlayerName);

public record RetrieveGameSessions(GameSessionCount NumberOfGameSessions);

public record JoinGameSession(GameSessionName GameSessionName, PlayerName JoiningPlayerName);

public record LeaveGameSession(GameSessionName GameSessionName, PlayerName LeavingPlayerOrHostName);

public record CheckSignalingInfoFromHost(GameSessionName GameSessionName, PlayerName RequestingPlayerName);

public record CheckSignalingInfoToPlayers(GameSessionName GameSessionName);

public record UpdateSignalingStep(GameSessionName GameSessionName, PlayerName RequestingPlayerName, InformationType InformationType, IceCandidate? IceCandidate, SessionDescription? SessionDescription);

public record UpdateSignalingStepFromHost(GameSessionName GameSessionName, PlayerName TargetPlayerName, InformationType InformationType, IceCandidate? IceCandidate, SessionDescription? SessionDescription);
