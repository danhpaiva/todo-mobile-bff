using TodoMobileBff.Models;

namespace TodoMobileBff.Services.Interfaces;

public interface ITodoClient
{
    Task<IEnumerable<TodoItem>> GetAllTodoItemsAsync();
    Task<TodoItem?> GetTodoItemByIdAsync(long id);
}
