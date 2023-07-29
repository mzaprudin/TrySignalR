using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("\nPlease specify the URL of SignalR Hub");
Console.WriteLine("Press enter to use default URL http://localhost:5128/echo");

var url = Console.ReadLine();

if (string.IsNullOrEmpty(url))
    url = @"http://localhost:5128/echo";

var hubConnection = new HubConnectionBuilder()
                         .WithUrl(url)
                         .Build();

hubConnection.On<string>("ReceiveMessage", 
    message => Console.WriteLine($"SignalR Hub Message: {message}"));

try
{
    await hubConnection.StartAsync();

    while (true)
    {
        var message = DateTime.UtcNow.ToString();

        Console.WriteLine($"Send: {message}");
        await hubConnection.SendAsync("BroadcastMessage", message);
        await Task.Delay(2000);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
