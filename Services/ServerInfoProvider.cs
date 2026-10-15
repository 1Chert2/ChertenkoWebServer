using ChertenkoWebServer.Contracts;
using Microsoft.Extensions.Options;

namespace ChertenkoWebServer.Services;

public sealed class ServerInfoProvider(
    IOptions<ServerInfoOptions> options,
    TimeProvider timeProvider) : IServerInfoProvider
{
    public ServerInfoResponse GetInfo()
    {
        var settings = options.Value;
        return new ServerInfoResponse(
            settings.ServerName,
            settings.Version,
            timeProvider.GetLocalNow().DateTime);
    }
}
