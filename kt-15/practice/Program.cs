using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Chat.Shared;

public class Program
{
    private static async Task Main()
    {
        HttpListener listener = new HttpListener();
        var builder = new UriBuilder("http", "localhost", 5000);
        Uri finalUri = builder.Uri;
        listener.Prefixes.Add(finalUri.ToString());
        listener.Start();

        Console.WriteLine($"Сервер запущен: {finalUri}");

        while (true)
        {
            HttpListenerContext context = await listener.GetContextAsync();
            _ = Task.Run(() => Obrabotka.HandleRequestAsync(context));
        }
    }

    public static class Obrabotka
    {
        private static readonly List<Message> _listforwork = new();
        private static readonly List<WebSocket> _clients = new();
        private static readonly object _lock = new();

        public static async Task HandleRequestAsync(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            
            if (!request.IsWebSocketRequest)
            {
                response.StatusCode = 400;
                response.Close();
                return;
            }

            WebSocketContext wsContext = await context.AcceptWebSocketAsync(subProtocol: null);
            WebSocket webSocket = wsContext.WebSocket;

            List<Message> historyCopy;
            lock (_lock)
            {
                _clients.Add(webSocket);
                historyCopy = _listforwork.ToList();
            }

            try
            {
                
                await SendJsonAsync(webSocket, historyCopy);

                
                byte[] buffer = new byte[4096];
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                while (webSocket.State == WebSocketState.Open)
                {
                   
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                 
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закрыто", CancellationToken.None);
                        break;
                    }

                    
                    string json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    MessageDto? dto = JsonSerializer.Deserialize<MessageDto>(json, options);

                    if (dto != null)
                    {
                        Message y = new Message
                        {
                            Nickname = dto.Nickname,
                            Text = dto.Text,
                            ReceivedAt = DateTime.Now
                        };

                        List<WebSocket> clientsCopy;
                        lock (_lock)
                        {
                            _listforwork.Add(y);
                            clientsCopy = _clients.ToList();
                        }

                    
                        var oneMessageList = new List<Message> { y };
                        foreach (var client in clientsCopy)
                        {
                            if (client.State == WebSocketState.Open)
                            {
                                await SendJsonAsync(client, oneMessageList);
                            }
                        }
                    }
                }
            }
            catch (WebSocketException ex)
            {
               Console.WriteLine("Клиент отключился {ex.Message}");
            }
            finally
            {
             
                lock (_lock)
                {
                    _clients.Remove(webSocket);
                }
                webSocket.Dispose();
            }
        }

        private static async Task SendJsonAsync<T>(WebSocket ws, T data)
        {
            string json = JsonSerializer.Serialize(data);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);
        }
    }
}