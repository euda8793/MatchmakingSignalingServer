
namespace MatchmakingSignalingServer.API;

public static class RequestHandler
{
    private static async Task<IResult> HandleUseCaseAndCatchExceptions<TUseCase>(IUseCaseHandler<TUseCase> handler, Func<TUseCase> getResult)
    {
        UseCaseResult result;

        try
        {
            var useCase = getResult();
            result = await handler.Handle(useCase);
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

    public static Delegate Create<TRequest, TUseCase>()
    {
        return async delegate (IUseCaseHandler<TUseCase> handler, TRequest request, IMapper mapper, IValidator<TRequest> validator)
        {
            return await HandleUseCaseAndCatchExceptions(handler, () =>
            {
                validator.ValidateAndThrow(request);
                return mapper.Map<TUseCase>(request);
            });
        };
    }

    public static Delegate Create<TUseCase>()
    {
        return async delegate (IUseCaseHandler<TUseCase> handler)
        {
            return await HandleUseCaseAndCatchExceptions(handler, () =>
            {
                var maybeUseCase = Activator.CreateInstance(typeof(TUseCase));
                if (!(maybeUseCase is TUseCase useCase))
                {
                    throw new InvalidCastException($"An error occured while constructing a {typeof(TUseCase)} using its empty constructor.");
                }
                return useCase;
            });
        };
    }
}
