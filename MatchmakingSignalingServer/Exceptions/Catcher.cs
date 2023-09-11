namespace MatchmakingSignalingServer.API.Exceptions;

public static class Catcher
{
    public static async Task<IResult> HandleAppAndDomainExceptions(Func<Task<UseCaseResult>> potentialErrorAction)
    {
        try
        {
            var useCaseResult = await potentialErrorAction();
            return Results.Ok(useCaseResult);
        }
        catch (ArgumentException e)
        {
            return Results.BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Results.BadRequest(e.Message);
        }
        catch (Exception ex)
        {
            //TODO: Log this better at some point
            Console.WriteLine(ex.ToString());
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
