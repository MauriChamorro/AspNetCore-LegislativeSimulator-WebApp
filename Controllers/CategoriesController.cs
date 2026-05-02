using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Models;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Filters.ActionFilters;
using WebAppMVC.Filters.ExceptionFilters;

namespace WebAppMVC.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    // GET
    public IActionResult Index()
    {
        var categories = _categoryRepository.GetCategories();
        return View(categories);
    }

    public IActionResult Edit(int? id)
    {
        ViewBag.Action = "Edit";
        var cat = _categoryRepository.GetCategoryById(id ?? 0);
        return View(cat);
    }

    [HttpPost]
    [CategoryEditExceptionFilter]
    public IActionResult Edit(Category category)
    {
        ViewBag.Action = "Edit";
        
        if (ModelState.IsValid)
        {
            _categoryRepository.UpdateCategory(category);
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
            _categoryRepository.AddCategory(category.Name, category.Description);
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Action = "Add";
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int categoryId)
    {
        _categoryRepository.DeleteCategory(categoryId);
        return RedirectToAction(nameof(Index));
    }
}