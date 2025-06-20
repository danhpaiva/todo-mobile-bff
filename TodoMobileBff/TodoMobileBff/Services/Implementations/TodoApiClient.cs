using System.Text.Json;
using TodoMobileBff.Models;
using TodoMobileBff.Services.Interfaces;

namespace TodoMobileBff.Services.Implementations;

public class TodoApiClient : ITodoClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseApiUrl = "http://localhost:5097/api/TodoItems";

    public TodoApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_baseApiUrl);
    }

    public async Task<IEnumerable<TodoItem>> GetAllTodoItemsAsync()
    {
        var response = await _httpClient.GetAsync(_baseApiUrl);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<TodoItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<TodoItem?> GetTodoItemByIdAsync(long id)
    {
        var response = await _httpClient.GetAsync($"{_baseApiUrl}/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TodoItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
