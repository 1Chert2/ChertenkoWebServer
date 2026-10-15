using ChertenkoWebServer.Contracts;
using ChertenkoWebServer.Infrastructure;
using ChertenkoWebServer.Services;

namespace ChertenkoWebServer.Endpoints;

public sealed class ArithmeticEndpointGroup(IMathService math) : IEndpointGroup
{
    public string Name => "Вычисления";

    private static object Err(string message, string? code = null) => new { error = new ApiError(message, code) };

    public void MapEndpoints(IEndpointRouteBuilder api)
    {
        api.MapGet("/sum", HandleSum).WithTags(Name);
        api.MapGet("/divide", HandleDivide).WithTags(Name);
    }

    private IResult HandleSum(HttpContext httpContext)
    {
        var operands = QueryNumberParser.ReadPair(httpContext.Request.Query, "a", "b");
        if (operands.Failed)
        {
            return TypedResults.BadRequest(Err(operands.ErrorMessage!, ApiErrorCodes.InvalidParameters));
        }

        return TypedResults.Ok(new SumResponse(
            operands.First,
            operands.Second,
            math.Add(operands.First, operands.Second)));
    }

    private IResult HandleDivide(HttpContext httpContext)
    {
        try
        {
            var operands = QueryNumberParser.ReadPair(httpContext.Request.Query, "a", "b");
            if (operands.Failed)
            {
                return TypedResults.BadRequest(Err(operands.ErrorMessage!, ApiErrorCodes.InvalidParameters));
            }

            if (operands.Second == 0d)
            {
                return TypedResults.BadRequest(Err(ApiErrorMessages.DivisionByZero, ApiErrorCodes.DivisionByZero));
            }

            if (!math.TryDivide(operands.First, operands.Second, out var quotient))
            {
                return TypedResults.BadRequest(Err(ApiErrorMessages.Overflow, ApiErrorCodes.Overflow));
            }

            return TypedResults.Ok(new DivideResponse(operands.First, operands.Second, quotient));
        }
        catch (Exception ex)
        {
            return TypedResults.Ok(Err(ex.Message, ApiErrorCodes.InternalServerError));
        }
    }
}