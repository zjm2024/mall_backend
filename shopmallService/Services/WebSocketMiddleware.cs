using StackExchange.Redis;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;


namespace shopmallService.Services
{

    public class WebSocketMiddleware
    {
        private static readonly Dictionary<string, WebSocket> _userConnections = new Dictionary<string, WebSocket>();
        private static readonly object _lockObject = new object();
        private readonly RequestDelegate _next;

        public WebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await HandleWebSocketAsync(webSocket, context);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task HandleWebSocketAsync(WebSocket webSocket, HttpContext context)
        {


            var personalId = context.Request.Query["personalId"].ToString();
            var dataType = context.Request.Query["dataType"].ToString();
            var key = personalId + ":" + dataType;
            if (string.IsNullOrEmpty(personalId))
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "缺少用户ID参数", CancellationToken.None);
                return;
            }

            lock (_lockObject)
            {
                _userConnections[key] = webSocket;
            }

            var buffer = new byte[1024 * 4];

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                        RemoveConnection(key);
                    }
                    else
                    {
                        // Handle incoming messages if needed
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        // Process message if needed
                    }
                }
            }
            catch (Exception)
            {
                RemoveConnection(key);
            }
        }

        private void RemoveConnection(string key)
        {
            lock (_lockObject)
            {
                if (_userConnections.ContainsKey(key))
                {
                    _userConnections.Remove(key);
                }
            }
        }

        public static async void SendMessageToUser(string key, object message)
        {
            WebSocket webSocket = null;
            lock (_lockObject)
            {
                _userConnections.TryGetValue(key, out webSocket);
            }

            if (webSocket != null && webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var json = JsonSerializer.Serialize(message);
                    var buffer = Encoding.UTF8.GetBytes(json);
                    var segment = new ArraySegment<byte>(buffer);
                    await webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch
                {
                    // Ignore errors when sending message
                }
            }
        }



        public static async void SendMessageToAll(object message)
        {
            var json = JsonSerializer.Serialize(message);
            var buffer = Encoding.UTF8.GetBytes(json);
            var segment = new ArraySegment<byte>(buffer);
            foreach (var client in _userConnections)
            {
                if (client.Value.State == WebSocketState.Open)
                {
                    await client.Value.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }

            }


        }
    }

}
