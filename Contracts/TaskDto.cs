namespace ChertenkoWebServer.Contracts;

public sealed record TaskDto(
    int Id,
    string Title,
    bool IsDone,
    DateTime CreatedAt,
    string? CategoryName);
