namespace TheButton.Mobile.Core;

public interface ICounterApiClient
{
    Task<int> IncrementAsync(string endpoint = "api/v2/counter");
}
