using WebAppMVC.Domain.Models;

namespace WebAppMVC.Domain.Repositories;

public interface ICategoryRepository
{
    void AddCategory(string name, string description);
    Category[] GetCategories();
    Category? GetCategoryById(int id);
    void UpdateCategory(Category catUpdated);
    void DeleteCategory(int id);
    bool Exist(Category category);
}