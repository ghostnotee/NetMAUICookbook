using c4_CreateDelete.Views;

namespace c4_CreateDelete;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(CustomerEditPage), typeof(CustomerEditPage));
    }
}