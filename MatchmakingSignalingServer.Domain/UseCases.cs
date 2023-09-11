namespace MatchmakingSignalingServer.Domain.UseCases;

public record UseCase;

public record HostGameSession(GameSessionName GameSessionName, PlayerName HostPlayerName) : UseCase;

public record RetrieveGameSessions(GameSessionCount NumberOfGameSessions) : UseCase;

public record JoinGameSession(GameSessionName GameSessionName, PlayerName JoiningPlayerName) : UseCase;

public record LeaveGameSession(GameSessionName GameSessionName, PlayerName LeavingPlayerOrHostName) : UseCase;

public record CheckSignalingInfoFromHost(GameSessionName GameSessionName, PlayerName RequestingPlayerName) : UseCase;

public record CheckSignalingInfoToPlayers(GameSessionName GameSessionName) : UseCase;

public record UpdateSignalingStep(GameSessionName GameSessionName, PlayerName RequestingPlayerName, InformationType InformationType, IceCandidate? IceCandidate, SessionDescription? SessionDescription) : UseCase;

public record UpdateSignalingStepFromHost(GameSessionName GameSessionName, PlayerName TargetPlayerName, InformationType InformationType, IceCandidate? IceCandidate, SessionDescription? SessionDescription) : UseCase;

public record NotifyPlayerConnected(GameSessionName GameSessionName, PlayerName ConnectedPlayerName) : UseCase;

public record ReconnectPlayerToGameSession(GameSessionName GameSessionName, PlayerName ReconnectingPlayerName) : UseCase;

public record UpdatePlayerTime(GameSessionName GameSessionName, PlayerName RefreshedPlayerName) : UseCase;
