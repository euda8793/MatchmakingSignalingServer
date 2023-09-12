var builder = WebApplication.CreateBuilder(args);

#region Add Services

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGameSessionData, GameSessionDBContext>();
builder.Services.AddUseCaseHandlers();

#endregion

#region Setup Swagger and Https Redirection

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#endregion

#region Static File Serving

app.UseDefaultFiles();

var contentTypeProvider = new FileExtensionContentTypeProvider();
contentTypeProvider.Mappings[".wasm"] = "application/wasm";
contentTypeProvider.Mappings[".pck"] = "application/octet-stream";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = contentTypeProvider,
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append(
             "Cross-Origin-Embedder-Policy", "require-corp");
        ctx.Context.Response.Headers.Append(
             "Cross-Origin-Opener-Policy", "same-origin");
    }
});

#endregion

#region Groups And Filtering

var gameSessionsRoutes = app.MapGroup("GameSessions");

if (!app.Environment.IsDevelopment())
{
    gameSessionsRoutes.AddEndpointFilter<ApiKeyEndpointFilter>();
}

#endregion

#region GET

gameSessionsRoutes.MapGet("/{GameSessionCount}", async (
    GameSessionCount GameSessionCount,
    IResolveUseCaseHandler resolver) => 
{ 
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        var useCase = new RetrieveGameSessions(GameSessionCount);
        return await resolver.ResolveAndHandleUseCase(useCase);
    });
}).Produces<GameSessionsResult>();

gameSessionsRoutes.MapGet("/{GameSessionName}/Player/{PlayerName}/SignalStatus", async (
    [AsParameters] ExistingPlayer ExistingPlayer,
    IResolveUseCaseHandler resolver) => 
{ 
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        var useCase = new CheckSignalFromHost(
            ExistingPlayer.GameSessionName,
            ExistingPlayer.PlayerName);

        return await resolver.ResolveAndHandleUseCase(useCase);
    });
}).Produces<PlayerSignalingStepResult>();

gameSessionsRoutes.MapGet("/{GameSessionName}/SignalStatus", async (
    [AsParameters] ExistingGameSession ExistingGameSession,
    IResolveUseCaseHandler resolver) => 
{ 
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        var useCase = new CheckSignalWithPlayers(
            ExistingGameSession.GameSessionName);

        return await resolver.ResolveAndHandleUseCase(useCase);
    });
}).Produces<HostSignalingStepResult>();

#endregion

#region POST

gameSessionsRoutes.MapPost("/", async (
    NewGameSession NewGameSession,
    IResolveUseCaseHandler resolver) =>
{
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        var useCase = new HostGameSession(
            GameSessionName.Create(NewGameSession.GameSessionName),
            PlayerName.Create(NewGameSession.PlayerName));

        return await resolver.ResolveAndHandleUseCase(useCase);
    });
});


gameSessionsRoutes.MapPost("/{GameSessionName}", async (
    [AsParameters] ExistingGameSession ExistingGameSession,
    [FromBody] NewPlayer NewPlayer, 
    IResolveUseCaseHandler resolver) =>
{
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        var useCase = new JoinGameSession(
            ExistingGameSession.GameSessionName,
            PlayerName.Create(NewPlayer.PlayerName));

        return await resolver.ResolveAndHandleUseCase(useCase);
    });
});

#endregion

#region PUT

gameSessionsRoutes.MapPut("/{GameSessionName}/Player/{PlayerName}", async (
    [AsParameters] ExistingPlayer ExistingPlayer,
    IResolveUseCaseHandler resolver) =>
{
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        var useCase = new ReconnectPlayerToGameSession(
            ExistingPlayer.GameSessionName,
            ExistingPlayer.PlayerName);

        return await resolver.ResolveAndHandleUseCase(useCase);
    });
});

gameSessionsRoutes.MapPut("/{GameSessionName}/Player/{PlayerName}/Connected", async (
    [AsParameters] ExistingPlayer ExistingPlayer,
    IResolveUseCaseHandler resolver) =>
{
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        var notifyPlayerUseCase = new NotifyPlayerConnected(
            ExistingPlayer.GameSessionName,
            ExistingPlayer.PlayerName);

        await resolver.ResolveAndHandleUseCase(notifyPlayerUseCase);

        var refreshPlayerUseCase = new UpdatePlayerTime(
            ExistingPlayer.GameSessionName,
            ExistingPlayer.PlayerName);

        return await resolver.ResolveAndHandleUseCase(refreshPlayerUseCase);
    });
});

gameSessionsRoutes.MapPut("/{GameSessionName}/Player/{PlayerName}/PlayerAccept", async (
    [AsParameters] ExistingPlayer ExistingPlayer,
    [FromBody] NetworkInfo NetworkInfo,
    IResolveUseCaseHandler resolver) =>
{
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        var useCase = new AcceptSignalAsPlayer(
            ExistingPlayer.GameSessionName,
            ExistingPlayer.PlayerName,
            NetworkInfo.InformationType,
            NetworkInfo.IceCandidate,
            NetworkInfo.SessionDescription);

        return await resolver.ResolveAndHandleUseCase(useCase);
    });
});

gameSessionsRoutes.MapPut("/{GameSessionName}/Player/{PlayerName}/HostAccept", async (
    [AsParameters] ExistingPlayer ExistingPlayer,
    [FromBody] NetworkInfo NetworkInfo,
    IResolveUseCaseHandler resolver) =>
{
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        var useCase = new AcceptSignalAsHost(
            ExistingPlayer.GameSessionName,
            ExistingPlayer.PlayerName,
            NetworkInfo.InformationType,
            NetworkInfo.IceCandidate,
            NetworkInfo.SessionDescription);

        return await resolver.ResolveAndHandleUseCase(useCase);
    });
});

#endregion

#region DELETE

gameSessionsRoutes.MapDelete("/{GameSessionName}/Player/{PlayerName}", async (
    [AsParameters] ExistingPlayer ExistingPlayer,
    IResolveUseCaseHandler resolver) =>
{
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        var useCase = new LeaveGameSession(
            ExistingPlayer.GameSessionName,
            ExistingPlayer.PlayerName);

        return await resolver.ResolveAndHandleUseCase(useCase);
    });
});

#endregion

app.Run();
