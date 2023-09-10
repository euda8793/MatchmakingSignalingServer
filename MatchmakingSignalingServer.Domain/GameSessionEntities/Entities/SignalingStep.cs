namespace MatchmakingSignalingServer.Domain.GameSessionEntities;

public class SignalingStep
{ 
    public PlayerName Source { get; init; }

    public PlayerName Target { get; init; }

    public InformationType InformationType { get; init; }

    public SessionDescription? SessionDescription { get; init; }

    public IceCandidate? IceCandidate { get; init; }

    public GameSession GameSession { get; set; }

    public SignalingStep(PlayerName source, PlayerName target) 
    {
        Source = source;
        Target = target;
        InformationType = InformationType.WAITING_FOR_INFORMATION;
    }
}
