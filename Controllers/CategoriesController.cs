using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Filters;
using WebAppMVC.Filters.ActionFilters;
using WebAppMVC.Filters.ExceptionFilters;
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
        ViewBag.Action = "Edit";
        
        var cat = StaticCategoriesRepositories.GetCategoryById(id ?? 0);
        return View(cat);
    }

    [HttpPost]
    [CategoryEditExceptionFilter]
    public IActionResult Edit(Category category)
    {
        ViewBag.Action = "Edit";
        
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
        ViewBag.Action = "Add";
        
        return View();
    }

    [HttpPost]
    [CategoryAddFilter]
    public IActionResult Add(Category category)
    {
        if (ModelState.IsValid)
        {
            StaticCategoriesRepositories.AddCategory(category.Name, category.Description);
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Action = "Add";
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int categoryId)
    {
        StaticCategoriesRepositories.DeleteCategory(categoryId);
        return RedirectToAction(nameof(Index));
    }
}