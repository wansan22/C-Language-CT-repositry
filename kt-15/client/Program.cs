using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Chat.Shared;

public class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        string nickname = Console.ReadLine()!;
        Console.Clear();


        using var client = new ClientWebSocket();
        await client.ConnectAsync(new Uri("ws://127.0.0.1:5000/"), CancellationToken.None);

        _ = Task.Run(() => ReceiveMessagesAsync(client));

     
        while (client.State == WebSocketState.Open)
        {
            string? text = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(text)) continue;

            var dto = new MessageDto
            {
                Nickname = nickname,                                                
                Text = text
            };

            string json = JsonSerializer.Serialize(dto);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            await client.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                endOfMessage: true,
                CancellationToken.None);
        }
    }

    private static async Task ReceiveMessagesAsync(ClientWebSocket client)
    {
        byte[] buffer = new byte[4096];
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        try
        {
            while (client.State == WebSocketState.Open)
            {
               
                WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                string json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var messages = JsonSerializer.Deserialize<List<Message>>(json, options);

                if (messages != null && messages.Count > 0)
                {
                    foreach (var i in messages)
                    {
                        Console.WriteLine($"{i.ReceivedAt} {i.Nickname} : {i.Text}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Соединение разорвано: {ex.Message}");
        }
    }
}