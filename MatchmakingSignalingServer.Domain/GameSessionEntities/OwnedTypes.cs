namespace MatchmakingSignalingServer.Domain.GameSessionEntities.OwnedTypes;

public enum ConnectionState
{
    WAITING_FOR_OFFER = 0,
    HANDSHAKE = 1,
    CONNECTED = 2,
    IS_HOST = 3
}

public enum InformationType
{
    WAITING_FOR_INFORMATION = 0,
    SESSION_DESCRIPTION = 1,
    ICE_CANDIDATE = 2,
}

public static class IntEnum
{
    public static T Create<T>(int index) where T : Enum
    {
        if (Enum.IsDefined(typeof(T), index))
        {
            return (T)(object)index;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(T));
        }
    }
}

public record GameSessionCount(int Count)
{
    public const int Minimum = 1;
    public const int Maximium = 100;
    
    public static GameSessionCount Create(int count)
    {
        if (count < Minimum || count > Maximium) throw new ArgumentOutOfRangeException($"{nameof(GameSessionCount)} - {nameof(count)}");
        return new GameSessionCount(count);
    }
}

public record GameSessionName(string Name)
{
    public const int MaxGameSessionNameChars = 128;
    public const int MinGameSessionNameChars = 12;
    public static GameSessionName Create(string? name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
        if (name.Count() > MaxGameSessionNameChars || name.Count() < MinGameSessionNameChars) throw new ArgumentOutOfRangeException(nameof(name));
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

public record SessionDescription(string SessionType, string Sdp)
{
    public static SessionDescription With(string? sessionType, string? sdp)
    {
        if (string.IsNullOrEmpty(sessionType)) throw new ArgumentNullException(nameof(sessionType));
        if (string.IsNullOrEmpty(sdp)) throw new ArgumentNullException(nameof(sdp));

        return new SessionDescription(sessionType, sdp);
    }

}

public record PlayerName(string Name)
{
    public const int MaxPlayerNameChars = 64;
    public const int MinPlayerNameChars = 8;

    public static PlayerName Create(string? name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
        if (name.Count() > MaxPlayerNameChars || name.Count() < MinPlayerNameChars) throw new ArgumentOutOfRangeException(nameof(name));
        return new PlayerName(name);
    }
}

