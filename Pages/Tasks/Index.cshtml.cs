using ChertenkoWebServer.Contracts;
using ChertenkoWebServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChertenkoWebServer.Pages.Tasks;

/// <summary>
/// Страница задач: табличный SSR-вывод с пагинацией, фильтрацией и сортировкой.
/// Изменяющие действия (toggle, delete) работают по PRG + TempData.
/// </summary>
public sealed class IndexModel(ITaskStore store) : PageModel
{
    public List<TaskDto> Items { get; private set; } = [];
    public List<Category> Categories { get; private set; } = [];

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 5;

    [BindProperty(SupportsGet = true)]
    public int? CategoryId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SortBy { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public int TotalCount { get; private set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public async Task OnGetAsync()
    {
        await LoadAsync();
    }

    /// <summary>
    /// PRG: отметка выполнения задачи → редирект на GET.
    /// </summary>
    public async Task<IActionResult> OnPostToggleAsync(int id, bool isDone)
    {
        var updated = await store.ToggleDoneAsync(id, isDone);
        TempData["SuccessMessage"] = updated is null
            ? $"Задача #{id} не найдена"
            : $"Задача #{id} помечена как {(isDone ? "выполненная" : "невыполненная")}";

        return RedirectToPage();
    }

    /// <summary>
    /// PRG: удаление задачи → редирект на GET.
    /// </summary>
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var deleted = await store.RemoveAsync(id);
        TempData["SuccessMessage"] = deleted
            ? $"Задача #{id} удалена"
            : $"Задача #{id} не найдена";

        return RedirectToPage();
    }

    private async Task LoadAsync()
    {
        if (CurrentPage < 1)
        {
            CurrentPage = 1;
        }
        if (PageSize < 1 || PageSize > 100)
        {
            PageSize = 5;
        }

        Items = await store.GetPageAsync(CategoryId, SortBy, CurrentPage, PageSize);
        TotalCount = await store.CountAsync(CategoryId, Search);
        Categories = await store.GetCategoriesAsync();
    }
}