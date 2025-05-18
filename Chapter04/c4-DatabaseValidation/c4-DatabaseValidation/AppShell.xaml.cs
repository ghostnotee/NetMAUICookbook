using c4_DatabaseValidation.Views;

namespace c4_DatabaseValidation;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(CustomerEditPage), typeof(CustomerEditPage));
		Routing.RegisterRoute(nameof(CustomerDetailPage), typeof(CustomerDetailPage));
	}
}
