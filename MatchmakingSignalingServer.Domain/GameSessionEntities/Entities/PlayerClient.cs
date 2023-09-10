namespace MatchmakingSignalingServer.Domain.GameSessionEntities;

public class PlayerClient
{
    public PlayerName PlayerName { get; set; }

    public ConnectionState ConnectionState { get; set; }

    public GameSession GameSession { get; set; }
    
    public PlayerClient(PlayerName playerName, bool isHost = false) 
    {
        PlayerName = playerName;
        ConnectionState = isHost ? ConnectionState.IS_HOST : ConnectionState.WAITING_FOR_OFFER;
    }
}
