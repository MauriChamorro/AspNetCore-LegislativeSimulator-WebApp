using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Models.Products;
using WebAppMVC.Domain.Services;
using WebAppMVC.Infrastructure.Repositories.StaticRepositories;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    
    // GET
    public IActionResult Index()
    {
        var products = StaticProductRepository.GetProducts(loadCategories:true);
        return View(products);
    }
    
    public IActionResult Edit(int? id)
    {
        var prodVm = new ProductViewModel
        {
            //avoid app crashes
            Product = StaticProductRepository.GetProductById(id ?? 0) ?? new Product(),
            Categories = StaticCategoriesRepositories.GetCategories()
        };
        
        ViewBag.Action = "Edit";
        return View(prodVm);
    }

    [HttpPost]
    public IActionResult Edit(ProductViewModel productVm)
    {
        if (ModelState.IsValid)
        {
            _productService.UpdateProduct(productVm);
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Action = "Edit";
        _productService.UpdateCategoriesFor(productVm);
        return View(productVm);
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        ViewBag.Action = "Add";
        var newProductTemplate = new ProductViewModel
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
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int productId)
    {
        StaticProductRepository.Delete(productId);
        return RedirectToAction(nameof(Index));
    }
}