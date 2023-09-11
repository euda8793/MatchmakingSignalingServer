namespace MatchmakingSignalingServer.Domain.GameSessionEntities;

public class SignalingStep
{
    public PlayerName Source { get; set; }

    public PlayerName Target { get; set; }

    public InformationType InformationType { get; set; }

    public SessionDescription? SessionDescription { get; set; }

    public IceCandidate? IceCandidate { get; set; }

    public GameSession GameSession { get; set; }

    public SignalingStep() { }

    public SignalingStep(PlayerName source, PlayerName target)
    {
        Source = source;
        Target = target;
        InformationType = InformationType.WAITING_FOR_INFORMATION;
    }

    public void Update(InformationType informationType,  SessionDescription? sessionDescription, IceCandidate? iceCandidate)
    {
        if (InformationType == InformationType.ICE_CANDIDATE && iceCandidate == null) throw new ArgumentNullException(nameof(iceCandidate));
        if (InformationType == InformationType.SESSION_DESCRIPTION && sessionDescription == null) throw new ArgumentNullException(nameof(sessionDescription));
        if (InformationType == InformationType.WAITING_FOR_INFORMATION && sessionDescription != null && iceCandidate != null) throw new ArgumentOutOfRangeException(nameof(informationType));

        InformationType = informationType;
        IceCandidate = iceCandidate;
        SessionDescription = sessionDescription;
    }  
}
