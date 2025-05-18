using c4_UiValidation.Views;

namespace c4_UiValidation;

public partial class AppShell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(CustomerEditPage), typeof(CustomerEditPage));
        Routing.RegisterRoute(nameof(CustomerDetailPage), typeof(CustomerDetailPage));
    }
}