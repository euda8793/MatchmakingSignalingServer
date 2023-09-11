
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

//app.UseDefaultFiles();

//var contentTypeProvider = new FileExtensionContentTypeProvider();
//contentTypeProvider.Mappings[".wasm"] = "application/wasm";
//contentTypeProvider.Mappings[".pck"] = "application/octet-stream";

//app.UseStaticFiles(new StaticFileOptions
//{
//    ContentTypeProvider = contentTypeProvider,
//    OnPrepareResponse = ctx =>
//    {
//        ctx.Context.Response.Headers.Append(
//             "Cross-Origin-Embedder-Policy", "require-corp");
//        ctx.Context.Response.Headers.Append(
//             "Cross-Origin-Opener-Policy", "same-origin");
//    }
//});

#endregion

var gameSessionsRoutes = app.MapGroup("GameSessions");

if (!app.Environment.IsDevelopment())
{
    gameSessionsRoutes.AddEndpointFilter<ApiKeyEndpointFilter>();
}

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

gameSessionsRoutes.MapGet("/{GameSessionName}/Player/{PlayerName}/SignalingStep", async (
    [AsParameters] ExistingPlayer ExistingPlayer,
    IResolveUseCaseHandler resolver) => 
{ 
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        var useCase = new CheckSignalingInfoFromHost(
            ExistingPlayer.GameSessionName,
            ExistingPlayer.PlayerName);

        return await resolver.ResolveAndHandleUseCase(useCase);
    });
}).Produces<PlayerSignalingStepResult>();

gameSessionsRoutes.MapGet("/{GameSessionName}/SignalingStep", async (
    [AsParameters] ExistingGameSession ExistingGameSession,
    IResolveUseCaseHandler resolver) => 
{ 
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        var useCase = new CheckSignalingInfoToPlayers(
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

gameSessionsRoutes.MapPut("/{GameSessionName}/Player/{PlayerName}/Refresh", async (
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

#endregion

#region PATCH

gameSessionsRoutes.MapPatch("/{GameSessionName}/PlayerToHost/{PlayerName}/IceCandidate", async (
    [AsParameters] ExistingPlayer ExistingPlayer,
    [FromBody] IceCandidate IceCandidate,
    IResolveUseCaseHandler resolver) =>
{
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        IceCandidate.Validate();

        var useCase = new UpdateSignalingStep(
            ExistingPlayer.GameSessionName,
            ExistingPlayer.PlayerName,
            InformationType.ICE_CANDIDATE,
            IceCandidate,
            null);

        return await resolver.ResolveAndHandleUseCase(useCase);
    });
});

gameSessionsRoutes.MapPatch("/{GameSessionName}/PlayerToHost/{PlayerName}/SessionDescription", async (
    [AsParameters] ExistingPlayer ExistingPlayer,
    [FromBody] SessionDescription SessionDescription,
    IResolveUseCaseHandler resolver) =>
{
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        SessionDescription.Validate();

        var useCase = new UpdateSignalingStep(
            ExistingPlayer.GameSessionName,
            ExistingPlayer.PlayerName,
            InformationType.SESSION_DESCRIPTION,
            null,
            SessionDescription);

        return await resolver.ResolveAndHandleUseCase(useCase);
    });
});

gameSessionsRoutes.MapPatch("/{GameSessionName}/HostToPlayer/{PlayerName}/IceCandidate", async (
    [AsParameters] ExistingPlayer ExistingPlayer,
    [FromBody] IceCandidate IceCandidate,
    IResolveUseCaseHandler resolver) =>
{
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        IceCandidate.Validate();

        var useCase = new UpdateSignalingStepFromHost(
            ExistingPlayer.GameSessionName,
            ExistingPlayer.PlayerName,
            InformationType.ICE_CANDIDATE,
            IceCandidate,
            null);

        return await resolver.ResolveAndHandleUseCase(useCase);
    });
});

gameSessionsRoutes.MapPatch("/{GameSessionName}/HostToPlayer/{PlayerName}/SessionDescription", async (
    [AsParameters] ExistingPlayer ExistingPlayer,
    [FromBody] SessionDescription SessionDescription,
    IResolveUseCaseHandler resolver) =>
{
    return await Catcher.HandleAppAndDomainExceptions(async () =>
    {
        SessionDescription.Validate();

        var useCase = new UpdateSignalingStepFromHost(
            ExistingPlayer.GameSessionName,
            ExistingPlayer.PlayerName,
            InformationType.SESSION_DESCRIPTION,
            null,
            SessionDescription);

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
