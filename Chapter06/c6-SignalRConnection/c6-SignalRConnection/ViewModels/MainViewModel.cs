using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;

namespace c6_SignalRConnection.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<BidData> _bids = [];

    private HubConnection _hubConnection = null!;
    private bool _isBidAccepted;

    [RelayCommand]
    private async Task Initialize()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://zggwkbtz-7128.euw.devtunnels.ms/auction")
            .Build();
        _hubConnection.On<BidData>("BidReceived", bid => { Bids.Insert(0, bid); });
        await _hubConnection.StartAsync();
    }

    [RelayCommand(CanExecute = nameof(CanAcceptBid))]
    private async Task AcceptBid(BidData bid)
    {
        await _hubConnection.InvokeCoreAsync("AcceptBid", [bid.Bidder]);
        _isBidAccepted = true;
    }

    private bool CanAcceptBid() => !_isBidAccepted;
}

public class BidData(string bidder, decimal price)
{
    public string Bidder { get; set; } = bidder;
    public decimal Price { get; set; } = price;
}