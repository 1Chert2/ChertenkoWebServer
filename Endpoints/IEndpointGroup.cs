namespace ChertenkoWebServer.Endpoints;

public interface IEndpointGroup
{
    string Name { get; }

    void MapEndpoints(IEndpointRouteBuilder api);
}
