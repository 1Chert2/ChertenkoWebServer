namespace ChertenkoWebServer.Contracts;

public sealed record TaskUpdateRequest(string? Title, bool? IsDone);
