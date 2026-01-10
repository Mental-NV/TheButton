using System.Net;
using TheButton.Mobile.Core;
using TheButton.Mobile.Infrastructure;
using Moq;
using Moq.Protected;

namespace TheButton.Mobile.IntegrationTests;

[TestClass]
public class CounterApiClientTests
{
    [TestMethod]
    public async Task IncrementAsync_PostsToV2Endpoint_ParsesValue()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\"value\": 20}")
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

        var client = new CounterApiClient(httpClient);

        // Act
        var result = await client.IncrementAsync();

        // Assert
        Assert.AreEqual(20, result);

        handlerMock.Protected().Verify(
           "SendAsync",
           Times.Exactly(1),
           ItExpr.Is<HttpRequestMessage>(req =>
              req.Method == HttpMethod.Post
              && req.RequestUri.ToString().EndsWith("api/v2/counter") // V2 Check
           ),
           ItExpr.IsAny<CancellationToken>()
        );
    }
}
