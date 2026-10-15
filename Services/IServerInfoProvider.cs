using ChertenkoWebServer.Contracts;

namespace ChertenkoWebServer.Services;

public interface IServerInfoProvider
{
    ServerInfoResponse GetInfo();
}
