namespace WebAppMVC.Models.Repositories;

public static class StaticCategoriesRepositories
{
    private static readonly List<Category> _categories = new()
    {
        new Category { Id = 1, Name = "Agua", Description = "Manejo de las emociones" },
        new Category { Id = 2, Name = "Tierra", Description = "Perseverancia" },
        new Category { Id = 3, Name = "Aire", Description = "Pensamientos" },
        new Category { Id = 4, Name = "Fuego", Description = "Acción-Reacción" }
    };

    public static void AddCategory(string name, string description)
    {
        if (_categories.Count > 0)
        {
            var maxId = _categories.Max(x => x.Id);
            _categories.Add(new Category { Id = maxId + 1, Name = name, Description = description });
        }
        else
        {
            _categories.Add(new Category { Id = 1, Name = name, Description = description });
        }
    }

    public static Category[] GetCategories()
    {
        return _categories.ToArray();
    }

    public static Category? GetCategoryById(int id)
    {
        var cat = _categories.FirstOrDefault(x => x.Id == id);
        return cat != null
            ? new Category { Id = cat.Id, Name = cat.Name, Description = cat.Description }
            : null;
    }

    public static void UpdateCategory(Category catUpdated)
    {
        var cat = _categories.FirstOrDefault(x => x.Id == catUpdated.Id);
        if (cat != null)
        {
            cat.Name = catUpdated.Name;
            cat.Description = catUpdated.Description;
        }
    }

    public static void DeleteCategory(int id)
    {
        var cat = _categories.FirstOrDefault(x => x.Id == id);
        if (cat != null)
            _categories.Remove(cat);
    }

    public static bool Exist(Category category) => 
        _categories.Any(x => x.Name == category.Name);
}