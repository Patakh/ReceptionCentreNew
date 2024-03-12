using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;
using ReceptionCentreNew.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.SignalR;

namespace ReceptionCentreNew.Controllers
{
    public class SocketController : Controller
    {
        static int localPort = 8787; // порт приема сообщений
        static int remotePort = 8787; // порт для отправки сообщений  
        static int port = 8787;
        public static string _User;
        static private string guid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        static SHA1 sha1 = SHA1.Create();
        static Socket listeningSocket;
        private readonly IHubContext<NotificationHub> HubContext;
        public SignInManager<ApplicationUser> SignInManager;
        public SocketController(IHubContext<NotificationHub> hubContext, SignInManager<ApplicationUser> signInManager)
        {
            SignInManager = signInManager;
            HubContext = hubContext;
            _User = SignInManager.Context.User.Identity.Name;
        }

        // GET: Socket
        public IActionResult Index()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();
            }
            return Json("x");
        }
        private static void Listen()
        {
            try
            {
                IPEndPoint localIP = new(IPAddress.Parse("127.0.0.1"), localPort);
                listeningSocket.Bind(localIP);

                while (true)
                {
                    StringBuilder builder = new();
                    int bytes = 0;
                    byte[] data = new byte[256];

                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

                    do
                    {
                        bytes = listeningSocket.ReceiveFrom(data, ref remoteIp);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (listeningSocket.Available > 0);
                    IPEndPoint remoteFullIp = remoteIp as IPEndPoint;

                    Console.WriteLine("{0}:{1} - {2}", remoteFullIp.Address.ToString(),
                                                    remoteFullIp.Port, builder.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();
            }
        }
        private static void Close()
        {
            if (listeningSocket != null)
            {
                listeningSocket.Shutdown(SocketShutdown.Both);
                listeningSocket.Close();
                listeningSocket = null;
            }
        }
        public JsonResult Listener()
        {
            string res = "";
            StringBuilder builder = new();
            IPEndPoint ipPoint = new(IPAddress.Parse("192.168.35.120"), port);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(10);
                res += "Сервер запущен. Ожидание подключений...";
                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    int bytes = 0;
                    byte[] data = new byte[256];
                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Default.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);
                    res += builder.ToString();
                    string message = "ваше сообщение доставлено";
                    data = Encoding.Default.GetBytes(message);
                    if (!builder.ToString().Contains("HTTP"))
                    {
                        handler.Send(data);
                    }
                    return Json(res + "/bbb");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult SocketJitsi()
        {
            if (!serverSocket.IsBound)
            {
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8889));
                serverSocket.Listen(128);
                serverSocket.BeginAccept(null, 0, OnAccept, null);
            }
            _User = SignInManager.Context.User.Identity.Name;
            return Json("server socket jitsi -" + serverSocket.IsBound);
        }
        public async void OnAccept(IAsyncResult result)
        {
            var user = User;
            byte[] buffer = new byte[1024];
            byte[] ansver = new byte[1024]; ;
            try
            {
                Socket client = null;
                string headerResponse = "";
                if (serverSocket != null && serverSocket.IsBound)
                {
                    client = serverSocket.EndAccept(result);
                    var i = client.Receive(buffer);
                    headerResponse = (Encoding.UTF8.GetString(buffer)).Substring(0, i);
                }
                var response = "0";
                if (client != null)
                {
                    if (headerResponse.Contains("HTTP"))
                    {
                        var key = headerResponse.Replace("ey:", "`")
                                  .Split('`')[1]
                                  .Replace("\r", "").Split('\n')[0]
                                  .Trim();
                        var test1 = AcceptKey(ref key);
                        var newLine = "\r\n";
                        response = @$"HTTP/1.1 101 Switching Protocols{newLine}
                                 Upgrade: websocket{newLine}
                                 Connection: Upgrade{newLine}
                                 Sec-WebSocket-Accept:{test1}{newLine}{newLine}""";

                        client.Send(Encoding.UTF8.GetBytes(response));

                        var i = client.Receive(buffer);

                        ansver = Encoding.UTF8.GetBytes("Получи ответ++ от сервера! 89999999999");
                        ansver = ansver.Where(w => w != 0).ToArray();
                        var sendx = new byte[ansver.Length + 2];
                        sendx[0] = 0x81;
                        sendx[1] = byte.Parse(ansver.Length.ToString());
                        for (int j = 2; j < ansver.Length + 2; j++)
                            sendx[j] = ansver[j - 2];

                        var subA = SubArray(buffer, 0, i);
                        string _as = Encoding.Unicode.GetString(subA, 0, subA.Length);

                        var sss = DecodeWebsocketFrame(subA);
                        var sendy = new byte[sss[0].Length + 2];
                        sendy[0] = 0x81;
                        sendy[1] = byte.Parse(sss[0].Length.ToString());
                        for (int k = 2; k < sss[0].Length + 2; k++)
                            sendy[k] = sss[0][k - 2];

                        client.Send(sendy);

                        string phone = BitConverter.ToString(sss[0]);
                        await signalRAlerts(phone.Replace("'", "").Replace("-3", "").TrimStart('3'));
                    }
                    else
                    {
                        var subA = SubArray<byte>(buffer, 0, buffer.Length);
                        string _as = Encoding.UTF8.GetString(subA, 0, subA.Length);
                        string __as = _as.Replace("\0", "");
                        client.Send(Encoding.UTF8.GetBytes(_as));
                        await signalRAlerts(__as);
                    }

                }
            }
            finally
            {
                if (serverSocket != null && serverSocket.IsBound)
                    serverSocket.BeginAccept(null, 0, OnAccept, null);
            }
        }
        public static T[] SubArray<T>(T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        private static string AcceptKey(ref string key) =>
              Convert.ToBase64String(ComputeHash(key + guid));
        private static byte[] ComputeHash(string str) =>
              sha1.ComputeHash(Encoding.ASCII.GetBytes(str));
        static List<byte[]> DecodeWebsocketFrame(byte[] bytes)
        {
            List<byte[]> ret = new();
            int offset = 0;
            while (offset + 6 < bytes.Length)
            {
                int len = bytes[offset + 1] - 0x80;

                if (len <= 125)
                {
                    byte[] key = [bytes[offset + 2], bytes[offset + 3], bytes[offset + 4], bytes[offset + 5]];
                    byte[] decoded = new byte[len];
                    for (int i = 0; i < len; i++)
                    {
                        int realPos = offset + 6 + i;
                        decoded[i] = (byte)(bytes[realPos] ^ key[i % 4]);
                    }
                    offset += 6 + len;
                    ret.Add(decoded);
                }
                else
                {
                    int a = bytes[offset + 2];
                    int b = bytes[offset + 3];
                    len = (a << 8) + b;
                    //Debug.Log("Length of ws: " + len);

                    byte[] key = [bytes[offset + 4], bytes[offset + 5], bytes[offset + 6], bytes[offset + 7]];
                    var decoded = new byte[len];
                    for (int i = 0; i < len; i++)
                    {
                        int realPos = offset + 8 + i;
                        decoded[i] = (byte)(bytes[realPos] ^ key[i % 4]);
                    }

                    offset += 8 + len;
                    ret.Add(decoded);
                }
            }
            return ret;
        }

        [AllowAnonymous]
        public async Task signalRAlerts(string message)
        {
            await HubContext.Clients.User(_User).SendAsync("incomingCall", message);
        }
    }
}