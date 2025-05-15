using c4_DetailAndEditing.Views;

namespace c4_DetailAndEditing;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(CustomerEditPage), typeof(CustomerEditPage));
        Routing.RegisterRoute(nameof(CustomerDetailPage), typeof(CustomerDetailPage));
    }
}