using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChertenkoWebServer.Pages;

/// <summary>
/// Главная страница: калькулятор, эхо-тест, информация о сервере, Task Manager (AJAX).
/// </summary>
public sealed class IndexModel : PageModel
{
    public void OnGet()
    {
        // Страница полностью клиентская (AJAX), серверная модель не требуется.
    }
}