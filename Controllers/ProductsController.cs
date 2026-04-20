using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Models;
using WebAppMVC.Models.Repositories;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Controllers;

public class ProductsController : Controller
{
    // GET
    public IActionResult Index()
    {
        var products = StaticProductRepository.GetProducts(loadCategories:true);
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
        var newProductTemplate = new ProductViewModel()
        {
            Categories = StaticCategoriesRepositories.GetCategories(),
            Product = new Product()
        };
        return View(newProductTemplate);
    }

    [HttpPost]
    public IActionResult Add(ProductViewModel productVm)
    {
        ViewBag.Action = "Add";
        if (ModelState.IsValid)
        {
            StaticProductRepository.AddProduct(productVm.Product);
            return RedirectToAction(nameof(Index));
        }
        productVm.Categories = StaticCategoriesRepositories.GetCategories();
        return View(productVm);
    }
    
    public IActionResult Delete(int productId)
    {
        StaticProductRepository.Delete(productId);
        return RedirectToAction(nameof(Index));
    }
}