using WebAppMVC.Domain.Services;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Infrastructure.Repositories.StaticRepositories;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Services;

public class ProductService: IProductService
{
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public void UpdateCategories(ProductViewModel productVm)
    {
        productVm.Categories = _categoryRepository.GetCategories();

    }

    public void UpdateProduct(ProductViewModel productVm)
    { 
        StaticProductRepository.UpdateProduct(productVm.Product);
    }
}