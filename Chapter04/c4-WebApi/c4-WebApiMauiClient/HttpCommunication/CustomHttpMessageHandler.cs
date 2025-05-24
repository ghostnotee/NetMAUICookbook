// ReSharper disable once CheckNamespace
namespace c4_WebApiMauiClient;

public static partial class CustomHttpMessageHandler
{
    static System.Net.Http.HttpMessageHandler PlatformHttpMessageHandler;
    public static System.Net.Http.HttpMessageHandler GetMessageHandler() => PlatformHttpMessageHandler;
}