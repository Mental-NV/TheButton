using System.Net.Http.Json;
using TheButton.Mobile.Core;

namespace TheButton.Mobile.Infrastructure;

public class ButtonApiClient : IButtonApiClient
{
    private readonly HttpClient _httpClient;

    public ButtonApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> ClickButtonAsync()
    {
        var response = await _httpClient.PostAsync("api/button/click", null);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ButtonResponse>();
        return result?.Value ?? throw new InvalidOperationException("API returned null response");
    }
}
