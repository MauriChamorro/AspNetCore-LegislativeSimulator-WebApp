using WebAppMVC.Domain.Models;
using WebAppMVC.Domain.Models.Products;

namespace WebAppMVC.Infrastructure.Repositories.StaticRepositories;

public static class StaticProductRepository
{
    private static List<Product> _products = new List<Product>
    {
        new Product
        {
            Id = 1, Name = "Squirtle ", Description = "Pokémon inicial de tipo agua de la primera generación (Kanto, #0007), caracterizado por ser una tortuga celeste de 0,5 m y 9 kg.", CategoryId = 1, Quantity = 1, Price = 500
        },
        new Product
        {
            Id = 2, Name = "Cubone", Description = "Pokémon Solitario de tipo Tierra, es conocido por llevar el cráneo de su madre fallecida en la cabeza.", CategoryId = 2, Quantity = 1, Price = 800
        }
    };

    public static void AddProduct(Product product)
    {
        if (_products.Count > 0)
        {
            var maxId = _products.Max(x => x.Id);
            product.Id = maxId + 1;
        }
        else
        {
            product.Id = 1;
            _products.Add(product);
        }
    }

    public static Product? GetProductById(int id, bool loadCategories = false)
    {
        var prod = _products.FirstOrDefault(x => x.Id == id);
        if (prod != null)
        {
            var newProd = new Product
            {
                Id = prod.Id,
                Name = prod.Name,
                Description = prod.Description,
                CategoryId = prod.CategoryId,
                Quantity = prod.Quantity,
                Price = prod.Price
            };

            if (loadCategories &&  newProd.CategoryId.HasValue)
                newProd.Category = StaticCategoriesRepositories.GetCategoryById(prod.CategoryId.Value);
            
            return newProd;
        }
        return null;
    }
    
    public static Product[] GetProducts(bool loadCategories = false)
    {
        var prods = _products.ToArray();
        if (loadCategories)
        {
            foreach (var prod in prods)
                if (prod.CategoryId.HasValue)
                    prod.Category = StaticCategoriesRepositories.GetCategoryById(prod.CategoryId.Value);
        }
        return prods;
    }

    public static void UpdateProduct(Product product)
    {
        var prod = _products.FirstOrDefault(x => x.Id == product.Id);
        if (prod != null)
        {
            prod.Name = product.Name;
            prod.Description = product.Description;
            prod.CategoryId = product.CategoryId;
            prod.Quantity = product.Quantity;
            prod.Price = product.Price;
        }
    }

    public static void Delete(int productId)
    {
        var prod = _products.FirstOrDefault(x => x.Id == productId);
        if (prod != null)
            _products.Remove(prod);
    }
}