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
        var cat = new Category { Id = id ?? 0 };
        return View(cat);
    }
}