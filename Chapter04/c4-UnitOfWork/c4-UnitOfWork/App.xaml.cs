using c4_UnitOfWork.DataAccess;
using SQLitePCL;

namespace c4_UnitOfWork;

public partial class App : Application
{
	public App()
	{
		Batteries_V2.Init();
		using var context = new CrmContext();
		context.Database!.EnsureCreated();
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}