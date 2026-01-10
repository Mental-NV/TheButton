using System.Net.Http.Json;
using TheButton.Mobile.Core;

namespace TheButton.Mobile.Infrastructure;

public class CounterApiClient : ICounterApiClient
{
    private readonly HttpClient _httpClient;

    public CounterApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> IncrementAsync(string endpoint = "api/v2/counter")
    {
        var response = await _httpClient.PostAsync(endpoint, null);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ButtonResponse>();
        return result?.Value ?? throw new InvalidOperationException("API returned null response");
    }
}
