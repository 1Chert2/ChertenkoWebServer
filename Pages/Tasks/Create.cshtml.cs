using System.ComponentModel.DataAnnotations;
using ChertenkoWebServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChertenkoWebServer.Pages.Tasks;

/// <summary>
/// Страница создания задачи. Классическая форма + PRG + TempData.
/// Серверная валидация через DataAnnotations (unobtrusive validation на клиенте).
/// </summary>
public sealed class CreateModel(ITaskStore store) : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Название обязательно")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 100 символов")]
    [NoForbiddenWords]
    [Display(Name = "Название задачи")]
    public string Title { get; set; } = "";

    [BindProperty]
    [Display(Name = "Категория")]
    public int? CategoryId { get; set; }

    public List<Category> Categories { get; private set; } = [];

    public async Task OnGetAsync()
    {
        Categories = await store.GetCategoriesAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Categories = await store.GetCategoriesAsync();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (CategoryId.HasValue && !Categories.Any(c => c.Id == CategoryId))
        {
            ModelState.AddModelError(nameof(CategoryId), "Выбранная категория не существует");
            return Page();
        }

        var created = await store.CreateAsync(Title.Trim(), CategoryId);

        TempData["SuccessMessage"] = $"Задача «{created.Title}» создана (ID: {created.Id})";
        return RedirectToPage("/Tasks/Index");
    }
}