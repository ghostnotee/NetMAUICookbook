using c4_DataCaching.Views;

namespace c4_DataCaching;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(CustomerEditPage), typeof(CustomerEditPage));
		Routing.RegisterRoute(nameof(CustomerDetailPage), typeof(CustomerDetailPage));
	}
}
