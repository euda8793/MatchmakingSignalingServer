namespace MatchmakingSignalingServer.Domain.GameSessionEntities;

public class GameSession
{
    public GameSessionName GameSessionName { get; set; }

    public ICollection<PlayerClient> Clients { get; init; } = new List<PlayerClient>();

    public ICollection<SignalingStep> Steps { get; init; } = new List<SignalingStep>();

    public GameSession(GameSessionName gameSessionName, PlayerName playerName) 
    { 
        GameSessionName = gameSessionName;
        Clients.Add(new PlayerClient(playerName, true));
    }

    private void ThrowIfPlayerExists(PlayerName playerName) 
    {
        if (Clients.Any(x => x.PlayerName == playerName))
        {
            throw new ArgumentException($"Player {nameof(playerName)} already exists in {nameof(GameSessionName)} {GameSessionName.Name}");
        }
    } 

    private void ResetSignalingStepsForPlayer(PlayerName playerName)
    {
        if (Steps is List<SignalingStep> steps)
        {
            steps.RemoveAll(x => x.Source == playerName || x.Target == playerName);
        }
    }
    private void AddSignalingStepsBetweenHostAndPlayer(PlayerName playerName, PlayerName hostName)
    {
        Steps.Add(new SignalingStep(hostName, playerName));
        Steps.Add(new SignalingStep(playerName, hostName));
    }

    public void AddPlayerToGameSession(PlayerName playerName)
    {
        ThrowIfPlayerExists(playerName);
        Clients.Add(new PlayerClient(playerName));
        ResetSignalingStepsForPlayer(playerName);

        var hostName = Clients.First(x => x.ConnectionState == ConnectionState.IS_HOST).PlayerName;
        AddSignalingStepsBetweenHostAndPlayer(playerName, hostName);
    }

    public bool IsPlayerHost(PlayerName leavingPlayerOrHostName)
    {
        var player = Clients.First(x => x.PlayerName == leavingPlayerOrHostName);
        return player.ConnectionState == ConnectionState.IS_HOST;
    }

    public void RemovePlayer(PlayerName playerName)
    {
        var player = Clients.First(x => x.PlayerName == playerName);
        if (Steps is List<SignalingStep> stepList) 
        {
            stepList.RemoveAll(x => x.Target == playerName || x.Source == playerName);
        }
        
        Clients.Remove(player);
    }

    public SignalingStep GetSignalingStepToPlayer(PlayerName requestingPlayerName)
    {
        var playerHost = Clients.First(x => x.ConnectionState == ConnectionState.IS_HOST);
        return Steps.First(x => x.Source == playerHost.PlayerName && x.Target == requestingPlayerName);
    }

    public List<SignalingStep> GetSignalingStepsForHost()
    {
        var playerHost = Clients.First(x => x.ConnectionState == ConnectionState.IS_HOST);
        return Steps.Where(x => x.Target == playerHost.PlayerName).ToList();
    }
}
