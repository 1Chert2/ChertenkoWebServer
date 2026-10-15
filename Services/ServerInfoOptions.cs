namespace ChertenkoWebServer.Services;

public sealed class ServerInfoOptions
{
    public const string SectionName = "ServerInfo";

    public string ServerName { get; set; } = "ChertenkoWebServer";

    public string Version { get; set; } = "1.0";
}
