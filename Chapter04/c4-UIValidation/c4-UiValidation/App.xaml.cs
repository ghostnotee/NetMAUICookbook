using c4_UiValidation.DataAccess;
using SQLitePCL;

namespace c4_UiValidation;

public partial class App
{
    public App()
    {
        Batteries_V2.Init();
        using var context = new CrmContext();
        context.Database.EnsureCreated();
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}