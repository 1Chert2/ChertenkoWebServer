using System.ComponentModel.DataAnnotations;
using ChertenkoWebServer.Contracts;
using ChertenkoWebServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChertenkoWebServer.Pages.Tasks;

/// <summary>
/// Страница редактирования задачи. Загрузка по ID, PRG + TempData.
/// </summary>
public sealed class EditModel(ITaskStore store) : PageModel
{
    [BindProperty]
    public int Id { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Название обязательно")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 100 символов")]
    [NoForbiddenWords]
    [Display(Name = "Название задачи")]
    public string Title { get; set; } = "";

    [BindProperty]
    [Display(Name = "Выполнена")]
    public bool IsDone { get; set; }

    [BindProperty]
    [Display(Name = "Категория")]
    public int? CategoryId { get; set; }

    public List<Category> Categories { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var task = await store.FindAsync(id);
        if (task is null)
        {
            TempData["ErrorMessage"] = $"Задача #{id} не найдена";
            return RedirectToPage("/Tasks/Index");
        }

        Id = task.Id;
        Title = task.Title;
        IsDone = task.IsDone;
        Categories = await store.GetCategoriesAsync();

        // Определяем CategoryId по названию категории (из DTO)
        var category = Categories.FirstOrDefault(c => c.Name == task.CategoryName);
        CategoryId = category?.Id;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Categories = await store.GetCategoriesAsync();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var dto = new UpdateTaskDto(Title.Trim(), IsDone, CategoryId);
        var updated = await store.UpdateAsync(Id, dto);

        if (updated is null)
        {
            TempData["ErrorMessage"] = $"Задача #{Id} не найдена";
            return RedirectToPage("/Tasks/Index");
        }

        TempData["SuccessMessage"] = $"Задача «{updated.Title}» обновлена";
        return RedirectToPage("/Tasks/Index");
    }
}