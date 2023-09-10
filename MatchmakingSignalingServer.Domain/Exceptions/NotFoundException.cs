namespace MatchmakingSignalingServer.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string msg) 
        : base($"Resource Not Found {msg}") { }
}
