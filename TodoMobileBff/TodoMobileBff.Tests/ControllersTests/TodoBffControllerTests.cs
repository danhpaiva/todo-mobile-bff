using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoMobileBff.Controllers;
using TodoMobileBff.Models;
using TodoMobileBff.Services.Interfaces;

namespace TodoMobileBff.Tests.ControllersTests
{
    public class TodoBffControllerTests
    {
        private readonly Mock<ITodoClient> _mockTodoApiClient;
        private readonly TodoBffController _controller;

        public TodoBffControllerTests()
        {
            _mockTodoApiClient = new Mock<ITodoClient>();
            _controller = new TodoBffController(_mockTodoApiClient.Object);
        }

        [Fact]
        public async Task GetTodos_ReturnsListOfTodoItemDtos_AndMapsCorrectly()
        {
            // Arrange
            var todoItemsFromApi = new List<TodoItem>
        {
            new TodoItem { Id = 1, Name = "Original Task 1", IsComplete = false },
            new TodoItem { Id = 2, Name = "Original Task 2", IsComplete = true }
        };

            _mockTodoApiClient.Setup(client => client.GetAllTodoItemsAsync())
                               .ReturnsAsync(todoItemsFromApi);

            // Act
            var result = await _controller.GetTodos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var todoDtos = Assert.IsAssignableFrom<IEnumerable<TodoItemDto>>(okResult.Value);

            var todoDtoList = todoDtos.ToList();
            Assert.Equal(2, todoDtoList.Count);

            // Verifica o primeiro item
            Assert.Equal(1, todoDtoList[0].Id);
            Assert.Equal("Original Task 1", todoDtoList[0].Title);
            Assert.False(todoDtoList[0].IsDone);

            // Verifica o segundo item
            Assert.Equal(2, todoDtoList[1].Id);
            Assert.Equal("Original Task 2", todoDtoList[1].Title);
            Assert.True(todoDtoList[1].IsDone);

            // Garante que o método do cliente foi chamado
            _mockTodoApiClient.Verify(client => client.GetAllTodoItemsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTodoById_ReturnsTodoItemDto_WhenFound()
        {
            // Arrange
            var todoItemFromApi = new TodoItem { Id = 1, Name = "Single Task", IsComplete = false};

            _mockTodoApiClient.Setup(client => client.GetTodoItemByIdAsync(1))
                               .ReturnsAsync(todoItemFromApi);

            // Act
            var result = await _controller.GetTodoById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var todoDto = Assert.IsType<TodoItemDto>(okResult.Value);

            Assert.Equal(1, todoDto.Id);
            Assert.Equal("Single Task", todoDto.Title);
            Assert.False(todoDto.IsDone);

            // Garante que o Secret não está presente no DTO e o método do cliente foi chamado
            _mockTodoApiClient.Verify(client => client.GetTodoItemByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetTodoById_ReturnsNotFound_WhenNotFound()
        {
            // Arrange
            _mockTodoApiClient.Setup(client => client.GetTodoItemByIdAsync(99))
                               .ReturnsAsync((TodoItem?)null); // Retorna null para simular não encontrado

            // Act
            var result = await _controller.GetTodoById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);

            // Garante que o método do cliente foi chamado
            _mockTodoApiClient.Verify(client => client.GetTodoItemByIdAsync(99), Times.Once);
        }
    }
}
