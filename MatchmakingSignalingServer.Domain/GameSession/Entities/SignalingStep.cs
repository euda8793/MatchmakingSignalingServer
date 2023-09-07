namespace MatchmakingSignalingServer.Domain.GameSession.Entities;

public class SignalingStep
{ 
    public PlayerName Source { get; set; }

    public PlayerName Target { get; set; }

    public InformationType InformationType { get; set; }

    public SessionDescription? SessionDescription { get; set; }

    public IceCandidate? IceCandidate { get; set; }

    public SignalingStep(
        PlayerName source,
        PlayerName target,
        InformationType informationType,
        SessionDescription? sessionDescription,
        IceCandidate? iceCandidate)
    {
        if (sessionDescription == null && iceCandidate == null)
            throw new ArgumentNullException($"{nameof(sessionDescription)} and {nameof(iceCandidate)}");
        if (sessionDescription != null && iceCandidate != null)
            throw new ArgumentOutOfRangeException($"{nameof(sessionDescription)} and {nameof(iceCandidate)}");

        Source = source;
        Target = target;
        InformationType = informationType;
        SessionDescription = sessionDescription;
        IceCandidate = iceCandidate;
    }
}
