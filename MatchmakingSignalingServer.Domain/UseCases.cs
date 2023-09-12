namespace MatchmakingSignalingServer.Domain.UseCases;

public record UseCase;

public record HostGameSession(GameSessionName GameSessionName, PlayerName HostPlayerName) : UseCase;

public record RetrieveGameSessions(GameSessionCount NumberOfGameSessions) : UseCase;

public record JoinGameSession(GameSessionName GameSessionName, PlayerName JoiningPlayerName) : UseCase;

public record LeaveGameSession(GameSessionName GameSessionName, PlayerName LeavingPlayerOrHostName) : UseCase;

public record CheckSignalFromHost(GameSessionName GameSessionName, PlayerName RequestingPlayerName) : UseCase;

public record CheckSignalWithPlayers(GameSessionName GameSessionName) : UseCase;

public record NotifyPlayerConnected(GameSessionName GameSessionName, PlayerName ConnectedPlayerName) : UseCase;

public record ReconnectPlayerToGameSession(GameSessionName GameSessionName, PlayerName ReconnectingPlayerName) : UseCase;

public record UpdatePlayerTime(GameSessionName GameSessionName, PlayerName RefreshedPlayerName) : UseCase;

public record AcceptSignalAsPlayer(GameSessionName GameSessionName, PlayerName RequestingPlayerName, InformationType InformationType, IceCandidate? IceCandidate, SessionDescription? SessionDescription) : UseCase;

public record AcceptSignalAsHost(GameSessionName GameSessionName, PlayerName ConnectedPlayerName, InformationType InformationType, IceCandidate? IceCandidate, SessionDescription? SessionDescription) : UseCase;
