namespace MatchmakingSignalingServer.API;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<HostGameSessionRequest, HostGameSession>()
            .ForCtorParam(nameof(HostGameSession.GameSessionName), m => m.MapFrom(s => GameSessionName.Create(s.GameSessionName)));

        CreateMap<string, RetrieveGameSession>()
            .ConvertUsing(s => new RetrieveGameSession(GameSessionName.Create(s)));
    }
}
