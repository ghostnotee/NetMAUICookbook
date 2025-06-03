using c6_SignalRServer;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();
app.MapHub<BidsHub>("/auction");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var hubContext = services.GetRequiredService<IHubContext<BidsHub>>();
    _ = Task.Run(async () =>
    {
        var rnd = new Random();
        var price = 1;
        while (BidsHub.IsAuctionRunning)
        {
            price = rnd.Next(price, price + 10);
            await hubContext.Clients.All.SendAsync("BidReceived",
                new { Bidder = $"Company{rnd.Next(1, 10)}", Price = price });
            await Task.Delay(rnd.Next(500, 3000));
        }
    });
}

app.Run();