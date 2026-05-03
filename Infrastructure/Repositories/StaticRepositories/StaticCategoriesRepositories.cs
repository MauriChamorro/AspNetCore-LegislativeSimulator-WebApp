using WebAppMVC.Domain.Models;
using WebAppMVC.Domain.Models.Products;

namespace WebAppMVC.Infrastructure.Repositories.StaticRepositories;

public static class StaticCategoriesRepositories
{
    private static readonly List<Category> Categories = new()
    {
        new Category { Id = 1, Name = "Agua", Description = "Manejo de las emociones" },
        new Category { Id = 2, Name = "Tierra", Description = "Perseverancia" },
        new Category { Id = 3, Name = "Aire", Description = "Pensamientos" },
        new Category { Id = 4, Name = "Fuego", Description = "Acción-Reacción" }
    };

    public static void AddCategory(string name, string description)
    {
        if (Categories.Count > 0)
        {
            var maxId = Categories.Max(x => x.Id);
            Categories.Add(new Category { Id = maxId + 1, Name = name, Description = description });
        }
        else
        {
            Categories.Add(new Category { Id = 1, Name = name, Description = description });
        }
    }

    public static Category[] GetCategories()
    {
        return Categories.ToArray();
    }

    public static Category? GetCategoryById(int id)
    {
        var cat = Categories.FirstOrDefault(x => x.Id == id);
        return cat != null
            ? new Category { Id = cat.Id, Name = cat.Name, Description = cat.Description }
            : null;
    }

    public static void UpdateCategory(Category catUpdated)
    {
        var cat = Categories.FirstOrDefault(x => x.Id == catUpdated.Id);
        if (cat != null)
        {
            cat.Name = catUpdated.Name;
            cat.Description = catUpdated.Description;
        }
    }

    public static void DeleteCategory(int id)
    {
        var cat = Categories.FirstOrDefault(x => x.Id == id);
        if (cat != null)
            Categories.Remove(cat);
    }

    public static bool Exist(Category category) => 
        Categories.Any(x => x.Name == category.Name);
}