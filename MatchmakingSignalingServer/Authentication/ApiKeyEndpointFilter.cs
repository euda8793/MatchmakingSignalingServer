namespace MatchmakingSignalingServer.Authentication;

public class ApiKeyEndpointFilter : IEndpointFilter
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ApiKeyEndpointFilter(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if(!context.HttpContext.Request.Headers.TryGetValue(EnvVarNames.API_KEY_HEADER_NAME, out var incomingApiKey))
        {
            return TypedResults.Unauthorized();
        }

        var apiKey = Environment.GetEnvironmentVariable(EnvVarNames.API_KEY_CONFIG_NAME);

        if (!apiKey?.Equals(incomingApiKey) ?? true)
        {
            return TypedResults.Unauthorized();
        }

        return await next(context);
    }
}
