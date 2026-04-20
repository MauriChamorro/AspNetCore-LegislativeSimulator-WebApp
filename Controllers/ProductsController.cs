using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Models.Repositories;

namespace WebAppMVC.Controllers;

public class ProductsController : Controller
{
    // GET
    public IActionResult Index()
    {
        var products = StaticProductRepository.GetProducts();
        return View(products);
    }
}