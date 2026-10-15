namespace ChertenkoWebServer.Contracts;

public sealed record ServerInfoResponse(string ServerName, string Version, DateTime CurrentTime);
