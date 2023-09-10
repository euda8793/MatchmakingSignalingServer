namespace MatchmakingSignalingServer.Domain;

public interface IGameSessionData : IDisposable
{
    public DbSet<GameSession> GameSessions { get; set; }
    public DbSet<PlayerClient> PlayerClients { get; set; }
    public DbSet<SignalingStep> SignalingSteps { get; set; } 
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
