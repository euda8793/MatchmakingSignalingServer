namespace MatchmakingSignalingServer.Domain.GameSessionEntities;

public class GameSession
{
    #region Properties

    public GameSessionName GameSessionName { get; set; }

    public ICollection<PlayerClient> Clients { get; init; } = new List<PlayerClient>();

    public ICollection<SignalingStep> Steps { get; init; } = new List<SignalingStep>();

    #endregion

    public GameSession() { }

    public GameSession(GameSessionName gameSessionName, PlayerName playerName) 
    { 
        GameSessionName = gameSessionName;
        Clients.Add(new PlayerClient(playerName, true));
    }

    #region Retrieval Helpers 

    private PlayerClient Host => Clients.First(x => x.ConnectionState == ConnectionState.IS_HOST);
    
    private PlayerClient Player(PlayerName playerName) => Clients.First(x => x.PlayerName == playerName);

    private SignalingStep StepSource(PlayerName source) => Steps.First(x => x.Source == source);

    private SignalingStep StepTarget(PlayerName target) => Steps.First(x => x.Target == target);

    private SignalingStep StepSourceTarget(PlayerName source, PlayerName target) => Steps.First(x => x.Source == source && x.Target == target);

    private List<SignalingStep> StepsTargetingHost => Steps.Where(x => x.Target == Host.PlayerName).ToList();

    #endregion

    #region Private

    private void ThrowIfPlayerExists(PlayerName playerName) 
    {
        if (Clients.Any(x => x.PlayerName == playerName))
        {
            throw new InvalidOperationException($"Player {nameof(playerName)} already exists in {nameof(GameSessionName)} {GameSessionName.Name}");
        }
    } 

    private void RemoveStepsForPlayer(PlayerName playerName)
    {
        if (Steps is List<SignalingStep> stepList) 
        {
            stepList.RemoveAll(x => x.Target == playerName || x.Source == playerName);
        }
    }

    private void InitiateSignalingProcess(PlayerName playerName)
    {
        var playerClient = Player(playerName);
        playerClient.ConnectionState = ConnectionState.CONNECTING;
        RemoveStepsForPlayer(playerName);
        Steps.Add(new SignalingStep(Host.PlayerName, playerName));
        playerClient.Refresh();
    }

    #endregion

    #region Public 

    public void AddPlayerToGameSession(PlayerName playerName)
    {
        ThrowIfPlayerExists(playerName);
        Clients.Add(new PlayerClient(playerName));
        InitiateSignalingProcess(playerName);
    }

    public bool IsPlayerHost(PlayerName leavingPlayerOrHostName)
    {
        return Player(leavingPlayerOrHostName).ConnectionState == ConnectionState.IS_HOST;
    }

    public void RemovePlayer(PlayerName playerName)
    {
        if (IsPlayerHost(playerName))
        {
            throw new InvalidOperationException($"Can't remove host client.");
        }
        
        var player = Player(playerName);
        RemoveStepsForPlayer(playerName);
        Clients.Remove(player);
    }

    public SignalingStep GetSignalingStepToPlayer(PlayerName requestingPlayerName)
    {
        return StepSourceTarget(Host.PlayerName, requestingPlayerName);
    }

    public List<SignalingStep> GetSignalingStepsForHost()
    {
        return StepsTargetingHost;
    }

    public void UpdatePlayerToConnected(PlayerName connectedPlayerName)
    {
        Player(connectedPlayerName).ConnectionState = ConnectionState.CONNECTED;
        RemoveStepsForPlayer(connectedPlayerName);
    }

    public void ReconnectPlayer(PlayerName reconnectingPlayerName)
    {
        if (IsPlayerHost(reconnectingPlayerName)) throw new ArgumentException("Host player can't be reconnected.");

        InitiateSignalingProcess(reconnectingPlayerName);
    }

    public void UpdatePlayerExpirationTime(PlayerName refreshedPlayerName)
    {
        Player(refreshedPlayerName).Refresh();
    }

    public void SetHostAsTarget(PlayerName targetPlayerName, InformationType informationType, IceCandidate? iceCandidate, SessionDescription? sessionDescription)
    {
        StepTarget(targetPlayerName).Update(targetPlayerName, Host.PlayerName, informationType, iceCandidate, sessionDescription);
    }

    public void SetConnectedPlayerAsTarget(PlayerName connectedPlayerName, InformationType informationType, IceCandidate? iceCandidate, SessionDescription? sessionDescription)
    {
        StepSource(connectedPlayerName).Update(Host.PlayerName, connectedPlayerName, informationType, iceCandidate, sessionDescription);
    }

    #endregion
}
