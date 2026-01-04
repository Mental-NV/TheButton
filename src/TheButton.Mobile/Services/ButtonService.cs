using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TheButton.Mobile.Services;

public interface IButtonService
{
    Task<int> ClickAsync(CancellationToken cancellationToken = default);
}

public sealed class ButtonService : IButtonService
{
    private readonly HttpClient _http;

    public ButtonService(HttpClient http) => _http = http;

    private sealed record CounterResponse(int Value);

    public async Task<int> ClickAsync(CancellationToken cancellationToken = default)
    {
        using var response = await _http.PostAsync("api/button/click", content: null, cancellationToken);
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<CounterResponse>(cancellationToken: cancellationToken);
        return payload?.Value ?? 0;
    }
}
