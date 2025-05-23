namespace c4_WebApiMauiClient.HttpCommunication;

public static partial class CustomHttpMessageHandler
{
    static readonly System.Net.Http.HttpMessageHandler PlatformHttpMessageHandler;
    public static System.Net.Http.HttpMessageHandler GetMessageHandler() => PlatformHttpMessageHandler;
}