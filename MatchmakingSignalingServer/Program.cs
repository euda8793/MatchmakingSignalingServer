var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<MatchesDbContext>(opt => opt.UseSqlLite(Environem));

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

//var matchesGroup = app.MapGroup("matches").AddEndpointFilter<ApiKeyEndpointFilter>();
//var connectionsGroup = app.MapGroup("connections").AddEndpointFilter<ApiKeyEndpointFilter>();

//matchesGroup.MapGet("/", () => { return "Hello world"; });
//matchesGroup.MapGet("/a", () => { return "Hello world"; });
//matchesGroup.MapGet("/b", () => { return "Hello world"; });
//connectionsGroup.MapGet("/", () => { return "Hello world"; });

app.Run();
