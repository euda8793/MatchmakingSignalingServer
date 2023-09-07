namespace MatchmakingSignalingServer.Domain.GameSession;

public class GameSession
{
    public required GameSessionName GameSessionName { get; set; }

    public required PlayerClient GameHost { get; set; }

    public required ICollection<PlayerClient> Clients { get; set; }
    
    public required ICollection<SignalingStep> Steps { get; set; }
}
