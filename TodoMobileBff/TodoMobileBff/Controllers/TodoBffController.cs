using Microsoft.AspNetCore.Mvc;
using TodoMobileBff.Models;
using TodoMobileBff.Services.Interfaces;

namespace TodoMobileBff.Controllers;

[ApiController]
[Route("api/bff/todos")]
public class TodoBffController : ControllerBase
{
    private readonly ITodoClient _todoApiClient;

    public TodoBffController(ITodoClient todoApiClient)
    {
        _todoApiClient = todoApiClient;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodos()
    {
        var todoItemsFromApi = await _todoApiClient.GetAllTodoItemsAsync();

        // Mapeia os dados da API externa para o DTO do BFF
        var todoItemDtos = todoItemsFromApi.Select(item => new TodoItemDto
        {
            Id = item.Id,
            Title = item.Name,
            IsDone = item.IsComplete
        }).ToList();

        return Ok(todoItemDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDto>> GetTodoById(long id)
    {
        var todoItemFromApi = await _todoApiClient.GetTodoItemByIdAsync(id);

        if (todoItemFromApi == null)
        {
            return NotFound();
        }

        var todoItemDto = new TodoItemDto
        {
            Id = todoItemFromApi.Id,
            Title = todoItemFromApi.Name,
            IsDone = todoItemFromApi.IsComplete
        };

        return Ok(todoItemDto);
    }
}
