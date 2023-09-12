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

    public void Update(PlayerName source, PlayerName target, InformationType informationType, IceCandidate? iceCandidate, SessionDescription? sessionDescription)
    {
        IntEnum.Validate(informationType);

        if (informationType == InformationType.WAITING_FOR_INFORMATION) throw new ArgumentOutOfRangeException(nameof(informationType));
        if (informationType == InformationType.ICE_CANDIDATE && iceCandidate == null) throw new ArgumentNullException(nameof(iceCandidate));
        if (informationType == InformationType.SESSION_DESCRIPTION && sessionDescription == null) throw new ArgumentNullException(nameof(sessionDescription));

        Target = target;
        Source = source;
        InformationType = informationType;
        IceCandidate = iceCandidate;
        SessionDescription = sessionDescription;
    }
}
