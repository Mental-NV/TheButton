using System.Globalization;
using System.Net.Http.Json;
using TheButton.Mobile.Core;

namespace TheButton.Mobile.Infrastructure.V3;

/// <summary>
/// Counter API client for the V3 endpoints.
/// </summary>
public sealed class CounterApiV3Client : ICounterApiClient
{
    private static readonly Uri _endpoint = new Uri("api/v3/counter", UriKind.Relative);
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="CounterApiV3Client"/> class.
    /// </summary>
    /// <param name="httpClient">The underlying HTTP client.</param>
    public CounterApiV3Client(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        this._httpClient = httpClient;
    }

    /// <inheritdoc />
    public async Task<int> IncrementAsync()
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, _endpoint);
        request.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture));

        using HttpResponseMessage response = await this._httpClient.SendAsync(request).ConfigureAwait(false);
        _ = response.EnsureSuccessStatusCode();

        ButtonResponse? result = await response.Content.ReadFromJsonAsync<ButtonResponse>().ConfigureAwait(false);
        return result?.Value ?? throw new InvalidOperationException("API returned null response.");
    }

    /// <inheritdoc />
    public async Task<int> GetAsync()
    {
        using HttpResponseMessage response = await this._httpClient.GetAsync(_endpoint).ConfigureAwait(false);
        _ = response.EnsureSuccessStatusCode();

        ButtonResponse? result = await response.Content.ReadFromJsonAsync<ButtonResponse>().ConfigureAwait(false);
        return result?.Value ?? throw new InvalidOperationException("API returned null response.");
    }
}
