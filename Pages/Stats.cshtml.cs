using ChertenkoWebServer.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChertenkoWebServer.Pages;

/// <summary>
/// Страница статистики: агрегированные данные по количеству задач с разбивкой по категориям.
/// Строго типизированная модель (PageModel).
/// </summary>
public sealed class StatsModel(ITaskStore store) : PageModel
{
    public List<CategoryStats> Stats { get; private set; } = [];

    public int TotalTasks => Stats.Sum(s => s.TotalTasks);
    public int TotalDone => Stats.Sum(s => s.DoneTasks);
    public int TotalPending => TotalTasks - TotalDone;
    public double DonePercent => TotalTasks == 0 ? 0 : Math.Round(TotalDone * 100.0 / TotalTasks, 1);

    public async Task OnGetAsync()
    {
        Stats = await store.GetStatsAsync();
    }
}