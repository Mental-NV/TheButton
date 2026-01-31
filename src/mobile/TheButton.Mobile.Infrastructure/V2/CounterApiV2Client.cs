using System.Net.Http.Json;
using TheButton.Mobile.Core;

namespace TheButton.Mobile.Infrastructure.V2;

/// <summary>
/// Counter API client for the V2 endpoints.
/// </summary>
public sealed class CounterApiV2Client : ICounterApiClient
{
    private static readonly Uri _endpoint = new Uri("api/v2/counter", UriKind.Relative);
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="CounterApiV2Client"/> class.
    /// </summary>
    /// <param name="httpClient">The underlying HTTP client.</param>
    public CounterApiV2Client(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        this._httpClient = httpClient;
    }

    /// <inheritdoc />
    public async Task<int> IncrementAsync()
    {
        using HttpResponseMessage response = await this._httpClient.PostAsync(_endpoint, content: null).ConfigureAwait(false);
        _ = response.EnsureSuccessStatusCode();

        ButtonResponse? result = await response.Content.ReadFromJsonAsync<ButtonResponse>().ConfigureAwait(false);
        return result?.Value ?? throw new InvalidOperationException("API returned null response.");
    }

    /// <inheritdoc />
    public Task<int> GetAsync()
    {
        throw new NotSupportedException("GET /api/v2/counter is not supported by the V2 API.");
    }
}
