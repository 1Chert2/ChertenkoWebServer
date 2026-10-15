using ChertenkoWebServer.Services;

namespace ChertenkoWebServer.Endpoints;

public sealed class ServerInfoEndpointGroup(IServerInfoProvider serverInfo) : IEndpointGroup
{
    public string Name => "Информация о сервере";

    public void MapEndpoints(IEndpointRouteBuilder api)
    {
        api.MapGroup("/server-info")
            .WithTags(Name)
            .MapGet("/", () => serverInfo.GetInfo());
    }
}
