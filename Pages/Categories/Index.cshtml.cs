using System.ComponentModel.DataAnnotations;
using ChertenkoWebServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChertenkoWebServer.Pages.Categories;

/// <summary>
/// Страница управления категориями: список + создание + удаление (CRUD, PRG).
/// </summary>
public sealed class IndexModel(ITaskStore store) : PageModel
{
    public List<Category> Categories { get; private set; } = [];

    [BindProperty]
    [Required(ErrorMessage = "Название категории обязательно")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Название от 2 до 50 символов")]
    [Display(Name = "Новая категория")]
    public string NewCategoryName { get; set; } = "";

    [BindProperty]
    public int? EditId { get; set; }

    [BindProperty]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Название от 2 до 50 символов")]
    [Display(Name = "Новое название")]
    public string? EditName { get; set; }

    public async Task OnGetAsync()
    {
        await LoadAsync();
    }

    /// <summary>
    /// PRG: создание категории.
    /// </summary>
    public async Task<IActionResult> OnPostCreateAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadAsync();
            return Page();
        }

        await store.CreateCategoryAsync(NewCategoryName.Trim());
        TempData["SuccessMessage"] = $"Категория «{NewCategoryName.Trim()}» создана";
        return RedirectToPage();
    }

    /// <summary>
    /// PRG: обновление названия категории.
    /// </summary>
    public async Task<IActionResult> OnPostUpdateAsync()
    {
        if (EditId is null || string.IsNullOrWhiteSpace(EditName))
        {
            TempData["ErrorMessage"] = "Не указаны данные для обновления";
            return RedirectToPage();
        }

        var updated = await store.UpdateCategoryAsync(EditId.Value, EditName.Trim());
        TempData["SuccessMessage"] = updated is null
            ? $"Категория #{EditId} не найдена"
            : $"Категория переименована в «{updated.Name}»";

        return RedirectToPage();
    }

    /// <summary>
    /// PRG: удаление категории (задачи сохранятся, CategoryId = null).
    /// </summary>
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var deleted = await store.RemoveCategoryAsync(id);
        TempData["SuccessMessage"] = deleted
            ? $"Категория #{id} удалена. Связанные задачи сохранены без категории."
            : $"Категория #{id} не найдена";

        return RedirectToPage();
    }

    private async Task LoadAsync()
    {
        Categories = await store.GetCategoriesAsync();
    }
}