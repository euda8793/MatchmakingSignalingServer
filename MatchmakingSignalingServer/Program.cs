var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<HostGameSessionRequestValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
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

var gameSessionsRoutes = app.MapGroup("gamesessions");

if (!app.Environment.IsDevelopment())
{
    gameSessionsRoutes.AddEndpointFilter<ApiKeyEndpointFilter>();
}

app.Run();
