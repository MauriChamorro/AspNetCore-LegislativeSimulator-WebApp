using WebAppMVC.Domain.Repositories;
using WebAppMVC.Models;

namespace WebAppMVC.Infrastructure.Repositories.InMemoryRepositories;

public class InMemoryCategoryRepository : ICategoryRepository
{
    private List<Category> _categories;

    public InMemoryCategoryRepository()
    {
        _categories = new List<Category>();
        _categories.Add(new() { Id = 1, Name = "Agua", Description = "Manejo de las emociones" });
        _categories.Add(new() { Id = 2, Name = "Tierra", Description = "Perseverancia" });
    }


    public void AddCategory(string name, string description)
    {
        if (_categories.Count > 0)
        {
            var maxId = _categories.Max(x => x.Id);
            var newCat = new Category { Id = maxId + 1, Name = name, Description = description };
            var categories = _categories.ToList();
            categories.Add(newCat);
            _categories =  new  List<Category>(categories);
        }
        else
        {
            _categories.Add(new Category { Id = 1, Name = name, Description = description });
        }
    }

    public Category[] GetCategories()
    {
        return _categories.ToArray();
    }

    public Category? GetCategoryById(int id)
    {
        var cat = _categories.FirstOrDefault(x => x.Id == id);
        return cat != null
            ? new Category { Id = cat.Id, Name = cat.Name, Description = cat.Description }
            : null;
    }

    public void UpdateCategory(Category catUpdated)
    {
        var cat = _categories.FirstOrDefault(x => x.Id == catUpdated.Id);
        if (cat != null)
        {
            cat.Name = catUpdated.Name;
            cat.Description = catUpdated.Description;
        }
    }

    public void DeleteCategory(int id)
    {
        var cat = _categories.FirstOrDefault(x => x.Id == id);
        if (cat != null)
            _categories.Remove(cat);
    }

    public bool Exist(Category category) =>
        _categories.Any(x => x.Name == category.Name);
}