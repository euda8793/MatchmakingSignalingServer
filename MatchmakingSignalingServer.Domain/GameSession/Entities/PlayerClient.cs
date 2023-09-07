namespace MatchmakingSignalingServer.Domain.GameSession.Entities;

public class PlayerClient
{
    public PlayerName PlayerName { get; set; }

    public ConnectionState ConnectionState { get; set; }

    public PlayerClient(PlayerName playerName, ConnectionState connectionState)
    {
        PlayerName = playerName;
        ConnectionState = connectionState;
    }
}
