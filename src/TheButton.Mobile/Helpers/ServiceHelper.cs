using System;

namespace TheButton.Mobile.Helpers;

public static class ServiceHelper
{
    public static T GetService<T>() where T : notnull =>
        Current.GetService(typeof(T)) is T service
            ? service
            : throw new InvalidOperationException($"Service not found: {typeof(T)}");

    public static IServiceProvider Current =>
        Application.Current?.Handler?.MauiContext?.Services
        ?? throw new InvalidOperationException("Maui service provider is not available.");
}
