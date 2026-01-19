using System.Net.Http.Json;
using TheButton.Mobile.Core;

namespace TheButton.Mobile.Infrastructure;

public class CounterApiV2Client : ICounterApiClient
{
    private readonly string _endpoint = "api/v2/counter";
    private readonly HttpClient _httpClient;

    public CounterApiV2Client(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> IncrementAsync()
    {
        var response = await _httpClient.PostAsync(_endpoint, null);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ButtonResponse>();
        return result?.Value ?? throw new InvalidOperationException("API returned null response");
    }

    public Task<int> GetAsync()
    {
        throw new NotSupportedException("GET /api/v2/counter is not supported by the V2 API.");
    }
}
