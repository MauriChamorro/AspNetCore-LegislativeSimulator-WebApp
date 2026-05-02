using WebAppMVC.ViewModels;

namespace WebAppMVC.Domain.Services;

public interface IProductService
{
    void UpdateCategories(ProductViewModel productVm);
    void UpdateProduct(ProductViewModel productVm);
}