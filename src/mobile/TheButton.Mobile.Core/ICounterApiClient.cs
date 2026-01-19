namespace TheButton.Mobile.Core;

public interface ICounterApiClient
{
    Task<int> IncrementAsync();
    Task<int> GetAsync();
}
