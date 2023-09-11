namespace MatchmakingSignalingServer.API.Requests;


public record ExistingGameSession(GameSessionName GameSessionName);

public record ExistingPlayer(GameSessionName GameSessionName, PlayerName PlayerName) : ExistingGameSession(GameSessionName);

public record NewPlayer(string PlayerName);

public record NewGameSession(string PlayerName, string GameSessionName);

public class NetworkInfo
{
    public IceCandidate? IceCandidate { get; set; }

    public SessionDescription? SessionDescription { get; set; }

    public InformationType InformationType { get; set; }
}
