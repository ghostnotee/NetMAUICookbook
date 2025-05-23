using System.Net.Http.Json;
using c4_WebApiMauiClient.HttpCommunication;

namespace c4_WebApiMauiClient.DataAccess;

public class CustomerWebRepository : IRepository<Customer>
{
    readonly HttpClient httpClient = WebApiHttpClient.Instance;
    readonly string CollectionName = "Customers";

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var response = await httpClient.GetAsync(CollectionName);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<Customer>>();
    }

    public async Task AddAsync(Customer item)
    {
        var response = await httpClient.PostAsJsonAsync(CollectionName, item);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Customer item)
    {
        var response = await httpClient.DeleteAsync($"{CollectionName}/{item.Id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<Customer> GetByIdAsync(int id)
    {
        var response = await httpClient.GetAsync($"{CollectionName}/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Customer>();
    }

    public async Task UpdateAsync(Customer item)
    {
        var response = await httpClient.PutAsJsonAsync($"{CollectionName}/{item.Id}", item);
        response.EnsureSuccessStatusCode();
    }
}