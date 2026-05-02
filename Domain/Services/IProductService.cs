using WebAppMVC.ViewModels;

namespace WebAppMVC.Domain.Services;

public interface IProductService
{
    void UpdateCategoriesFor(ProductViewModel productVm);
    void UpdateProduct(ProductViewModel productVm);
}