namespace MatchmakingSignalingServer.Domain.GameSession.OwnedTypes;

public enum ConnectionState
{
    HANDSHAKE = 0,
    CONNECTED = 1
}

public enum InformationType
{
    SESSION_DESCRIPTION = 0,
    ICE_CANDIDATE = 1
}

public record GameSessionName(string Name)
{
    private const int MaxGameSessionNameChars = 128;
    public static GameSessionName Create(string? name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
        if (name.Count() > MaxGameSessionNameChars) throw new ArgumentOutOfRangeException(nameof(name));
        return new GameSessionName(name);
    }
}

public record IceCandidate(string Media, string Index, string Name)
{
    public static IceCandidate With(string? media, string? index, string? name)
    {
        if (string.IsNullOrEmpty(media)) throw new ArgumentNullException(nameof(media));
        if (string.IsNullOrEmpty(index)) throw new ArgumentNullException(nameof(index));
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

        return new IceCandidate(media, index, name);
    }
}

public record SessionDescription(string Typ, string Sdp)
{
    public static SessionDescription With(string? type, string? sdp)
    {
        if (string.IsNullOrEmpty(type)) throw new ArgumentNullException(nameof(type));
        if (string.IsNullOrEmpty(sdp)) throw new ArgumentNullException(nameof(sdp));

        return new SessionDescription(type, sdp);
    }

}

public record PlayerName(string Name)
{
    private const int MaxPlayerNameChars = 64;

    public static PlayerName Create(string? name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
        if (name.Count() > MaxPlayerNameChars) throw new ArgumentOutOfRangeException(nameof(name));
        return new PlayerName(name);
    }
}

