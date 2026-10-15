using ChertenkoWebServer.Contracts;
using ChertenkoWebServer.Infrastructure;
using ChertenkoWebServer.Services;

namespace ChertenkoWebServer.Endpoints;

public sealed class EchoEndpointGroup(IEchoService echo) : IEndpointGroup
{
    public string Name => "Эхо";

    private static object Err(string message, string? code = null) => new { error = new ApiError(message, code) };

    public void MapEndpoints(IEndpointRouteBuilder api)
    {
        api.MapGroup("/echo")
            .WithTags(Name)
            .MapPost("/", HandleEcho);
    }

    private IResult HandleEcho(EchoRequest? request)
    {
        if (request?.Text is null)
        {
            return TypedResults.BadRequest(Err(ApiErrorMessages.MissingEchoText, ApiErrorCodes.InvalidParameters));
        }

        return TypedResults.Ok(echo.Analyze(request.Text));
    }
}