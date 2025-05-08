using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace c2_MvvmDependencyInjection;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .RegisterViewModels()
            .RegisterViews()
            .RegisterAppServices()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<MyViewModel>();
        return mauiAppBuilder;
    }

    private static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<MainPage>();
        return mauiAppBuilder;
    }

    private static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<IDummyService, DummyService>();
        return mauiAppBuilder;
    }
}

public class DiSource : IMarkupExtension
{
    public Type Type { get; set; } = null!;

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return Application.Current.MainPage.Handler.MauiContext.Services.GetService(Type);
    }
}