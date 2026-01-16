using System.Net.Http.Json;
using TheButton.Mobile.Core;

namespace TheButton.Mobile.Infrastructure;

public class CounterApiV3Client : ICounterApiClient
{
    private readonly string _endpoint = "api/v3/counter";
    private readonly HttpClient _httpClient;

    public CounterApiV3Client(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> IncrementAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _endpoint);
        request.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString());
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ButtonResponse>();
        return result?.Value ?? throw new InvalidOperationException("API returned null response");
    }
}
