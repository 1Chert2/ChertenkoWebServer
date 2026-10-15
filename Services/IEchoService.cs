using ChertenkoWebServer.Contracts;

namespace ChertenkoWebServer.Services;

public interface IEchoService
{
    EchoResponse Analyze(string text);
}
