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
        StaticCategoriesRepositories.UpdateCategory(category);
        return RedirectToAction(nameof(Index));
    }
}