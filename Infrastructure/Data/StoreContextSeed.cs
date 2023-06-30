using Core.Entities;
using Core.Entities.OrderAggregate;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var tasks = new Task[]
        {
            SeedBrandsAsync(context, path!),
            SeedTypesAsync(context, path!),
            SeedProductsAsync(context, path!),
            SeedDeliveryMethodsAsync(context, path!)
        };

        await Task.WhenAll(tasks);

        if(context.ChangeTracker.HasChanges())
            await context.SaveChangesAsync();
    }

    private static Task SeedDeliveryMethodsAsync(StoreContext context, string path)
    {
        if(context.DeliveryMethods.Any()) return Task.CompletedTask;
        var dmData = File.ReadAllText(path + @"/Data/SeedData/delivery.json");
        var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
        return context.DeliveryMethods.AddRangeAsync(methods!);
    }

    private static Task SeedProductsAsync(StoreContext context, string path)
    {
        if(context.Products.Any()) return Task.CompletedTask;
        var productsData = File.ReadAllText(path + @"/Data/SeedData/products.json");
        var products = JsonSerializer.Deserialize<List<Product>>(productsData);
        return context.Products.AddRangeAsync(products!);
    }

    private static Task SeedTypesAsync(StoreContext context, string path)
    {
        if(context.ProductTypes.Any()) return Task.CompletedTask;
        var typesData = File.ReadAllText(path + @"/Data/SeedData/types.json");
        var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
        return context.ProductTypes.AddRangeAsync(types!);
    }

    private static Task SeedBrandsAsync(StoreContext context, string path)
    {
        if(context.ProductBrands.Any()) return Task.CompletedTask;
        var brandsData = File.ReadAllText(path + @"/Data/SeedData/brands.json");
        var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
        return context.ProductBrands.AddRangeAsync(brands!);
    }
}
