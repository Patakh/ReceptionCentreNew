﻿using Microsoft.AspNetCore.SignalR;

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
public class NotificationHub : Hub, IHubContext
{
    // Подключение нового пользователя
    public void Connect(string userName)
    {
    }

    private readonly static ConnectionMapping<string> _connections = new();

    public IHubClients Clients => throw new NotImplementedException();

    public IGroupManager Groups => throw new NotImplementedException();
     
    public Dictionary<string, HashSet<string>> get_online_users()
    {
        return _connections.GetAllConnections();
    }
}