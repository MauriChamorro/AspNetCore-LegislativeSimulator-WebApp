using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Models;
using WebAppMVC.Models.Repositories;

namespace WebAppMVC.Controllers;

public class ProductsController : Controller
{
    // GET
    public IActionResult Index()
    {
        var products = StaticProductRepository.GetProducts(loadCategories:false);
        return View(products);
    }
    
    public IActionResult Edit(int? id)
    {
        ViewBag.Action = "Edit";
        
        var prod = StaticProductRepository.GetProductById(id ?? 0);
        return View(prod);
    }

    [HttpPost]
    public IActionResult Edit(Product product)
    {
        ViewBag.Action = "Edit";
        
        if (ModelState.IsValid)
        {
            StaticProductRepository.UpdateProduct(product);
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        ViewBag.Action = "Add";
        
        return View();
    }

    [HttpPost]
    public IActionResult Add(Product product)
    {
        ViewBag.Action = "Add";
        if (ModelState.IsValid)
        {
            StaticProductRepository.AddProduct(product);
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }
    
    public IActionResult Delete(int productId)
    {
        StaticProductRepository.Delete(productId);
        return RedirectToAction(nameof(Index));
    }
}