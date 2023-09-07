namespace MatchmakingSignalingServer.Domain.Handlers;

public interface ICommandHandler<in T>
{
    ValueTask Handle(T command, CancellationToken cancellationToken);
}
