using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

namespace MatchmakingSignalingServer.Domain.GameSessionEntities.ValueTypes;

public enum ConnectionState
{
    CONNECTING = 0,
    CONNECTED = 1,
    DISCONNECTED = 2,
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

public static class TypeAssist
{
    public static bool FalseIfThrows<T>(Func<T> getResult, out T? result)
    {
        try
        {
            result = getResult(); 
        }
        catch (Exception e) when (e is ArgumentNullException || e is FormatException || e is ArgumentOutOfRangeException)
        {
            result = default(T);
            return false;
        }

        return true;
    }
}

public record GameSessionCount(int Count)
{
    public const int Minimum = 1;
    public const int Maximium = 100;

    public void Validate() => Create(Count);

    public static GameSessionCount Create(int count)
    {
        if (count < Minimum || count > Maximium) throw new ArgumentOutOfRangeException($"{nameof(GameSessionCount)} - {nameof(count)}");
        return new GameSessionCount(count);
    }

    public static bool TryParse(string? value, IFormatProvider? formatProvider, out GameSessionCount? result) => 
        TypeAssist.FalseIfThrows(() => Create(int.Parse(value)), out result);
}

public record GameSessionName(string Name)
{
    public const int MaxGameSessionNameChars = 128;
    public const int MinGameSessionNameChars = 8;

    public void Validate() => Create(Name);

    public static GameSessionName Create(string? name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
        if (name.Count() > MaxGameSessionNameChars || name.Count() < MinGameSessionNameChars) throw new ArgumentOutOfRangeException(nameof(name));
        return new GameSessionName(name);
    }
    public static bool TryParse(string? value, IFormatProvider? formatProvider, out GameSessionName? result) => 
        TypeAssist.FalseIfThrows(() => Create(value), out result);
}

public record IceCandidate(string Media, string Index, string Name)
{
    public void Validate() => With(Media, Index, Name);

    public static IceCandidate With(string? media, string? index, string? name)
    {
        if (string.IsNullOrEmpty(media)) throw new ArgumentNullException(nameof(Media));
        if (string.IsNullOrEmpty(index)) throw new ArgumentNullException(nameof(Index));
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(Name));

        return new IceCandidate(media, index, name);
    }
}

public record SessionDescription(string SessionType, string Sdp)
{
    public void Validate() => With(SessionType, Sdp);   

    public static SessionDescription With(string? sessionType, string? sdp)
    {
        if (string.IsNullOrEmpty(sessionType)) throw new ArgumentNullException(nameof(sessionType));
        if (string.IsNullOrEmpty(sdp)) throw new ArgumentNullException(nameof(sdp));

        return new SessionDescription(sessionType, sdp);
    }

}

public record PlayerName(string Name)
{
    public const int MaxPlayerNameChars = 48;
    public const int MinPlayerNameChars = 6;

    public void Validate() => Create(Name);

    public static PlayerName Create(string? name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
        if (name.Count() > MaxPlayerNameChars || name.Count() < MinPlayerNameChars) throw new ArgumentOutOfRangeException(nameof(name));
        return new PlayerName(name);
    }

    public static bool TryParse(string? value, IFormatProvider? formatProvider, out PlayerName? result) => 
        TypeAssist.FalseIfThrows(() => Create(value), out result);
}

