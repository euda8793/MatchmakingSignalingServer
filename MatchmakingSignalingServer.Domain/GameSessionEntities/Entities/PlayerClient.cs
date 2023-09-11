namespace MatchmakingSignalingServer.Domain.GameSessionEntities;

public class PlayerClient
{
    public PlayerName PlayerName { get; set; }

    public ConnectionState ConnectionState { get; set; }

    public GameSession GameSession { get; set; }

    public DateTime Expiration { get; set; }
    
    public PlayerClient() { }
    public PlayerClient(PlayerName playerName, bool isHost = false) 
    {
        PlayerName = playerName;
        ConnectionState = isHost ? ConnectionState.IS_HOST : ConnectionState.CONNECTING;
        Refresh();
    }

    public void Refresh()
    {
        Expiration = DateTime.UtcNow.AddMinutes(5);
    }
}
