namespace TheButton.Mobile.Core;

public interface IButtonApiClient
{
    Task<int> ClickButtonAsync();
}
