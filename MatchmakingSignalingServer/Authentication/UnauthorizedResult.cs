namespace MatchmakingSignalingServer.Authentication;

public sealed class UnauthorizedResult : IResult, IStatusCodeHttpResult
{
    private readonly object _body;

    public UnauthorizedResult(object body)
    {
        _body = body;
    }

    public int? StatusCode => StatusCodes.Status401Unauthorized;

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        httpContext.Response.StatusCode = (int)StatusCode;
        
        if(_body is string s)
        {
            await httpContext.Response.WriteAsync(s);
        }

        await httpContext.Response.WriteAsJsonAsync(_body);
    }
}
