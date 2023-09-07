namespace MatchmakingSignalingServer.Infrastructure;

public class GameSessionDBContext : DbContext
{
    public DbSet<GameSession> GameSessions { get; set; }

    public GameSessionDBContext(DbContextOptions<GameSessionDBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameSession>(gs =>
        {
            gs.Property<int>("GameSessionId");

            gs.Property(x => x.GameSessionName)
            .HasConversion(x => x.Name, x => GameSessionName.Create(x));

            gs.HasMany(gs => gs.Clients)
            .WithOne()
            .IsRequired();

            gs.HasMany(gs => gs.Steps)
            .WithOne()
            .IsRequired();

            gs.HasOne(gs => gs.GameHost)
            .WithOne()
            .IsRequired();
        });

        modelBuilder.Entity<SignalingStep>(st =>
        {

        });

        modelBuilder.Entity<PlayerClient>(pc =>
        {

        });
    }
}