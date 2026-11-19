using System.Diagnostics.Contracts;
using System.Diagnostics.Metrics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using Chat.Shared;
using System.Net.Http.Json;
public class Program

{
    public static async Task Main()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("http://127.0.0.1:5000");
        
        
        string nickname = Console.ReadLine()!;
        Console.Clear();
        _ = Task.Run(() => ReceiveMessagesAsync(client));

        while (true)
        {
        
        string? text = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(text)) continue; 

    var dto = new MessageDto()    
    {
        Nickname = nickname, 
        Text = text
    };
    


    await client.PostAsJsonAsync("/api/send", dto);

        }
    }
    private static async Task ReceiveMessagesAsync(HttpClient client)
    {
        int count = 0;
        while (true)
        {
        try
            {
            var messages = await client.GetFromJsonAsync<List<Message>>($"/api/receive?count={count}");
        if (messages != null && messages.Count > 0)
            {
            count += messages.Count;
                foreach( var i in messages)
                    {
                        Console.WriteLine(i);
                    }
            }
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Ошибка не удалось найти сообщения {ex.Message}");
                await Task.Delay(5000);
            }
        }
        
    }
}