namespace MatchmakingSignalingServer.API.Exceptions
{
    public class UseCaseHandlerMissingException : ArgumentException
    {
        public UseCaseHandlerMissingException(Type t) 
            : base($"UseCaseHandler: {t.Name}") { }
    }
}
