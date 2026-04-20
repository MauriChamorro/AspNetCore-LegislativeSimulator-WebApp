using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Models;
using WebAppMVC.Models.Repositories;

namespace WebAppMVC.Controllers;

public class CategoriesController : Controller
{
    // GET
    public IActionResult Index()
    {
        var categories = StaticCategoriesRepositories.GetCategories();
        return View(categories);
    }

    public IActionResult Edit(int? id)
    {
        var cat = StaticCategoriesRepositories.GetCategoryById(id ?? 0);
        return View(cat);
    }

    [HttpPost]
    public IActionResult Edit(Category category)
    {
        if (ModelState.IsValid)
        {
            StaticCategoriesRepositories.UpdateCategory(category);
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(Category category)
    {
        if (ModelState.IsValid)
        {
            StaticCategoriesRepositories.AddCategory(category.Name, category.Description);
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    public IActionResult Delete(int categoryId)
    {
        StaticCategoriesRepositories.DeleteCategory(categoryId);
        return RedirectToAction(nameof(Index));
    }
}