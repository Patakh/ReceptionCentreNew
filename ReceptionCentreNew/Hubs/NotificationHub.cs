using Microsoft.AspNetCore.SignalR;

namespace ReceptionCentreNew.Hubs;
public class HubConnection
{
    public int Id { get; set; }
    public string ConnectionId { get; set; }
    public string SessionId { get; set; }
}
public class ConnectionMapping<T>
{
    private readonly Dictionary<T, HashSet<string>> _connections = new Dictionary<T, HashSet<string>>();
    public int Count
    {
        get
        {
            return _connections.Count;
        }
    }

    public void Add(T key, string connectionId)
    {
        lock (_connections)
        {
            HashSet<string> connections;
            if (!_connections.TryGetValue(key, out connections))
            {
                connections = new HashSet<string>();
                _connections.Add(key, connections);
            }

            lock (connections)
            {
                connections.Add(connectionId);
            }
        }
    }

    public IEnumerable<string> GetConnections(T key)
    {
        HashSet<string> connections;
        if (_connections.TryGetValue(key, out connections))
        {
            return connections;
        }

        return Enumerable.Empty<string>();
    }
    public Dictionary<T, HashSet<string>> GetAllConnections()
    {
        return _connections;
    }
    public void Remove(T key, string connectionId)
    {
        lock (_connections)
        {
            HashSet<string> connections;
            if (!_connections.TryGetValue(key, out connections))
            {
                return;
            }

            lock (connections)
            {
                connections.Remove(connectionId);

                if (connections.Count == 0)
                {
                    _connections.Remove(key);
                }
            }
        }
    }
}
public class NotificationHub : Hub
{
    private readonly static ConnectionMapping<string> _connections  = new();

    public override async Task OnConnectedAsync()
    {
        string name = Context.User.Identity.Name;

        _connections.Add(name, Context.ConnectionId);

          await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    
    
    {
        string name = Context.User.Identity.Name;

        _connections.Remove(name, Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendNotification(string message)
    {
        string name = Context.User.Identity.Name;

        await Clients.All.SendAsync("ReceiveNotification", name, message);
    }
    public async Task SendComment(string message)
    {
        await Clients.All.SendAsync("IncomingCall", message);
    }
    public async Task IncomingCall(string message)
    {
        string name = Context.User.Identity.Name;

        await Clients.User(name).SendAsync("incomingCall", message);
    }
    public Dictionary<string, HashSet<string>> GetOnlineUsers()
    {
        return _connections.GetAllConnections();
    }
}