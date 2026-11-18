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
    public class Program
{
    private static async Task Main()
    {
        
       HttpListener listener = new HttpListener();
       var builder = new UriBuilder("http","127.0.0.1",5000);
       Uri finalUri = builder.Uri;
       listener.Prefixes.Add(finalUri.ToString());
       listener.Start();

        while (true)
        {
            HttpListenerContext context = await listener.GetContextAsync();
            _ = Task.Run(() => Obrabotka.HandleRequestAsync(context));
        }
        
    }
    public static class Obrabotka
    {
        private static List<Message> _listforwork = new();  
        private static readonly object _lock = new();
        private static TaskCompletionSource<bool> _tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public static async Task HandleRequestAsync(HttpListenerContext context)
        {
            
            var request = context.Request;
            var response = context.Response;
            string path = request.Url?.AbsolutePath ?? "нету ссылки url";
            if (request.HttpMethod == "POST" && path == "/api/send")
            {
                using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                string body = await reader.ReadToEndAsync(); 
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };   
                MessageDto? dto = JsonSerializer.Deserialize<MessageDto>(body, options);
                if ( dto != null)
                {
                    Message y = new Message();
                    y.Nickname = dto.Nickname;
                    y.Text = dto.Text;
                    y.ReceivedAt = DateTime.Now;
                    
                    lock (_lock)
                    {
                        _listforwork.Add(y);
                        var rubilnik = _tcs.TrySetResult(true);
                        _tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);;
                    }
                    
                }
                
                response.StatusCode = 200;
                response.Close();
            }
            else if (request.HttpMethod == "GET" && path == "/api/receive")
            {
                int.TryParse(request.QueryString["count"], out int clientCount);
                List<Message> newmessage = new List<Message>();
                Task? waitTask = null;
                lock (_lock)
                {
                    if (_listforwork.Count <= clientCount)
                        {
                            waitTask = _tcs.Task;
                        }
                }
                    if (waitTask != null)
                        {
                            await Task.WhenAny(waitTask, Task.Delay(30000));
                        }
                        lock (_lock)
                        {
                            newmessage = _listforwork.Skip(clientCount).ToList();
                        }
                
                var x = JsonSerializer.Serialize<List<Message>>(newmessage);
                byte[] buffer = Encoding.UTF8.GetBytes(x);
                response.StatusCode = 200;
                await response.OutputStream.WriteAsync(buffer);
                response.Close();
            }
            else
            {
                response.StatusCode = 404;
                response.Close();
            }
        

        }
    }
}

