using System.Net;
using TheButton.Mobile.Core;
using TheButton.Mobile.Infrastructure.V2;
using Moq;
using Moq.Protected;

namespace TheButton.Mobile.IntegrationTests;

[TestClass]
public class CounterApiV2ClientTests
{
    [TestMethod]
    public async Task IncrementAsync_PostsToEndpoint_ParsesValue()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\"value\": 10}")
        };

        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost:5001/")
        };

        var client = new CounterApiV2Client(httpClient);

        // Act
        var result = await client.IncrementAsync();

        // Assert
        Assert.AreEqual(10, result);

        handlerMock.Protected().Verify(
           "SendAsync",
           Times.Exactly(1),
           ItExpr.Is<HttpRequestMessage>(req =>
              req.Method == HttpMethod.Post
              && req.RequestUri != null
              && req.RequestUri.ToString().EndsWith("api/v2/counter", StringComparison.Ordinal) // Check URL
           ),
           ItExpr.IsAny<CancellationToken>()
        );
    }

    [TestMethod]
    public void GetAsync_ThrowsNotSupportedException()
    {
        // Arrange
        var httpClient = new HttpClient();
        var client = new CounterApiV2Client(httpClient);

        // Act & Assert
        Assert.ThrowsExceptionAsync<NotSupportedException>(async () => await client.GetAsync());
    }
}
