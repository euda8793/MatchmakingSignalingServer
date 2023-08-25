using MatchmakingSignalingServer.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MatchesDbContext>(opt => opt.UseSqlLite(Environem))

var app = builder.Build();


app.MapGet("/", () => 
{ 
    return "Hello World"; 
});

app.Run();
