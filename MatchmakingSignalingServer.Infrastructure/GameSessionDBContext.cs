namespace MatchmakingSignalingServer.Infrastructure;

public class GameSessionDBContext : DbContext, IGameSessionData
{
    public DbSet<GameSession> GameSessions { get; set; }
    
    public DbSet<PlayerClient> PlayerClients { get; set; }

    public DbSet<SignalingStep> SignalingSteps { get; set; }

    public GameSessionDBContext() : base() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = Path.Join(path, "gamesessiondb.db");
        Console.WriteLine(dbPath);
        optionsBuilder.UseSqlite($"Data Source = {dbPath}"); 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameSession>(gs =>
        {
            gs.Property<int>("GameSessionId");

            gs.Property(x => x.GameSessionName)
            .HasConversion(x => x.Name, x => GameSessionName.Create(x))
            .IsRequired();

            gs.HasMany(gs => gs.Clients)
            .WithOne(p => p.GameSession);

            gs.HasMany(gs => gs.Steps)
            .WithOne(s => s.GameSession);
        });

        modelBuilder.Entity<SignalingStep>(sp =>
        {
            sp.Property<int>("SignalingStepId");

            sp.Property(x => x.Source)
            .HasConversion(x => x.Name, x => PlayerName.Create(x));

            sp.Property(x => x.Target)
            .HasConversion(x => x.Name, x => PlayerName.Create(x));

            sp.Property(x => x.InformationType)
            .HasConversion(x => (int)x, x => IntEnum.Create<InformationType>(x));

            sp.OwnsOne(x => x.SessionDescription);

            sp.OwnsOne(x => x.IceCandidate);

        });

        modelBuilder.Entity<PlayerClient>(pc =>
        {
            pc.Property<int>("PlayerClientId");

            pc.Property(x => x.PlayerName)
            .HasConversion(x => x.Name, x => PlayerName.Create(x));

            pc.Property(x => x.ConnectionState)
            .HasConversion(x => (int)x, x => IntEnum.Create<ConnectionState>(x));
        });
    }
}