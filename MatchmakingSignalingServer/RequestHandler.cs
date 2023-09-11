
namespace MatchmakingSignalingServer.API;

public static class RequestHandler
{
    private static async Task<IResult> HandleUseCaseAndCatchExceptions(IResolveUseCaseHandler resolver, Func<UseCase> map, Action? preCallSteps = null)
    {
        UseCaseResult result;

        try
        {
            if(preCallSteps != null)
            {
                preCallSteps();
            }

            var useCase = map();
            result = await resolver.ResolveAndHandleUseCase(useCase);
        }
        catch(ValidationException vE)
        {
            return TypedResults.BadRequest(vE.Errors);
        }
        catch(ArgumentException aE)
        {
            return TypedResults.BadRequest(aE.Message);

        }
        catch(NotFoundException nfe)
        {
            return TypedResults.NotFound(nfe.Message);
        }
        catch(InvalidOperationException ioe)
        {
            return TypedResults.NotFound(ioe);
        }
        catch(Exception ex)
        {
            // log ex
            return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);  
        }

        return TypedResults.Ok(result);
    }

    #region Public

    public static Delegate Create<TRequest>(Func<TRequest, UseCase> map)
    {
        return async delegate (IResolveUseCaseHandler resolver, TRequest request, IValidator<TRequest> validator)
        {
            return await HandleUseCaseAndCatchExceptions(
                resolver, 
                () => map(request), 
                () => validator.ValidateAndThrow(request));
        };
    }

    public static Delegate Create(Func<UseCase> map)
    {
        return async delegate (IResolveUseCaseHandler resolver)
        {
            return await HandleUseCaseAndCatchExceptions(resolver, map);
        };
    }

    #endregion
}

