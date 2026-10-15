using ChertenkoWebServer.Contracts;

namespace ChertenkoWebServer.Services;

public sealed class EchoService : IEchoService
{
    public EchoResponse Analyze(string text) =>
        new(text, text.ToUpperInvariant(), text.Length);
}
