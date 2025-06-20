using System.Net;
using System.Text;
using System.Text.Json;
using Moq;
using Moq.Protected;
using TodoMobileBff.Models;
using TodoMobileBff.Services.Implementations;

namespace TodoMobileBff.Tests.Tests
{
    public class TodoApiClientTests
    {
        [Fact]
        public async Task GetAllTodoItemsAsync_ReturnsExpectedItems_OnSuccess()
        {
            // Arrange
            var expectedItems = new List<TodoItem>
        {
            new TodoItem { Id = 1, Name = "Item 1", IsComplete = false},
            new TodoItem { Id = 2, Name = "Item 2", IsComplete = true}
        };
            var jsonResponse = JsonSerializer.Serialize(expectedItems);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected() // 'Protected' para métodos protegidos como SendAsync
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var client = new TodoApiClient(httpClient);

            // Act
            var result = await client.GetAllTodoItemsAsync();

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Equal(2, resultList.Count);
            Assert.Equal("Item 1", resultList[0].Name);
            Assert.Equal("Item 2", resultList[1].Name);

            // Verifica se a requisição GET foi feita corretamente
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(), // Garantimos que SendAsync foi chamado uma vez
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri != null && // Verifica se RequestUri não é null
                    req.RequestUri.ToString().Contains("https://api.example.com/todos") // Verifica a URL base
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetTodoItemByIdAsync_ReturnsExpectedItem_OnSuccess()
        {
            // Arrange
            var expectedItem = new TodoItem { Id = 10, Name = "Specific Item", IsComplete = false, Secret = "SpecificS" };
            var jsonResponse = JsonSerializer.Serialize(expectedItem);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri != null && req.RequestUri.ToString().Contains("/todos/10")), // Verifica a URL específica
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var client = new TodoApiClient(httpClient);

            // Act
            var result = await client.GetTodoItemByIdAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
            Assert.Equal("Specific Item", result.Name);

            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetTodoItemByIdAsync_ReturnsNull_OnNotFound()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var client = new TodoApiClient(httpClient);

            // Act
            var result = await client.GetTodoItemByIdAsync(99);

            // Assert
            Assert.Null(result);

            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetAllTodoItemsAsync_ThrowsHttpRequestException_OnUnsuccessfulStatusCode()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError // Simula um erro do servidor
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var client = new TodoApiClient(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => client.GetAllTodoItemsAsync());

            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
