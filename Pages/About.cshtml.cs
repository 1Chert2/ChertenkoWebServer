using ChertenkoWebServer.Contracts;
using ChertenkoWebServer.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChertenkoWebServer.Pages;

/// <summary>
/// Страница "О сервере": системная информация.
/// </summary>
public sealed class AboutModel(IServerInfoProvider serverInfo) : PageModel
{
    public ServerInfoResponse ServerInfo { get; private set; } = null!;

    public void OnGet()
    {
        ServerInfo = serverInfo.GetInfo();
    }
}