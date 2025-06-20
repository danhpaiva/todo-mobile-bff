namespace TodoMobileBff.Models;

public class TodoItemDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public bool IsDone { get; set; }
}
