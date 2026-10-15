namespace ChertenkoWebServer.Contracts;

public sealed record ApiError(
    string Message,
    string? Code = null);
