namespace MatchmakingSignalingServer.Authentication;

public class ApiKeyEndpointFilter : IEndpointFilter
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ApiKeyEndpointFilter(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if(!context.HttpContext.Request.Headers.TryGetValue(EnvVarNames.API_KEY_HEADER_NAME, out var incomingApiKey))
        {
            return new UnauthorizedResult(ResponseMessageText.NO_API_KEY_PROVIDED);
        }

        var apiKey = 
            _webHostEnvironment.IsDevelopment() ? 
            _configuration.GetValue<string>(EnvVarNames.API_KEY_CONFIG_NAME) : 
            Environment.GetEnvironmentVariable(EnvVarNames.API_KEY_CONFIG_NAME);

        if (!apiKey?.Equals(incomingApiKey) ?? true)
        {
            return new UnauthorizedResult(ResponseMessageText.WRONG_API_PROVIDED);
        }

        return await next(context);
    }
}
