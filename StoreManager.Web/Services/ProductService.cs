using System.Net.Http.Json;
using StoreManager.Web.Models;

namespace StoreManager.Web.Services;

public class ProductService(HttpClient http)
{
    private const string BasePath = "api/products";

    public async Task<List<Product>> GetAllAsync()
    {
        return await http.GetFromJsonAsync<List<Product>>(BasePath) ?? new List<Product>();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await http.GetFromJsonAsync<Product>($"{BasePath}/{id}");
    }

    public async Task<Product?> CreateAsync(Product product)
    {
        var response = await http.PostAsJsonAsync(BasePath, product);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Product>();
    }

    public async Task UpdateAsync(Product product)
    {
        var response = await http.PutAsJsonAsync($"{BasePath}/{product.Id}", product);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await http.DeleteAsync($"{BasePath}/{id}");
        response.EnsureSuccessStatusCode();
    }
}