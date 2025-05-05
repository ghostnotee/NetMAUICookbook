using CommunityToolkit.Mvvm.Messaging;

namespace c2_UiAndViewModelInteraction;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<Customer>(this, (r, customer) => { CustomersCollectionView.ScrollTo(customer); });
    }
}