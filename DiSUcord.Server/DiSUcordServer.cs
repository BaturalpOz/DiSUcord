using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class DiSUcordServer
{
    private TcpListener tcpListener;
    private List<ClientHandler> clients = new List<ClientHandler>();
    private List<string> subscribedClientsIF100 = new List<string>();
    private List<string> subscribedClientsSPS101 = new List<string>();
    private object lockObject = new object();

    public DiSUcordServer(IPAddress ipAddress, int port)
    {
        tcpListener = new TcpListener(ipAddress, port);
    }

    public void Start()
    {
        tcpListener.Start();
        Console.WriteLine($"Server started. Listening on {((IPEndPoint)tcpListener.LocalEndpoint).Port}");

        try
        {
            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                ClientHandler clientHandler = new ClientHandler(this, tcpClient);
                Thread clientThread = new Thread(new ThreadStart(clientHandler.HandleClient));
                clientThread.Start();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void BroadcastMessage(string sender, string message, List<string> subscribers)
    {
        lock (lockObject)
        {
            foreach (var client in clients)
            {
                if (subscribers.Contains(client.Username))
                {
                    client.SendMessage($"{sender}: {message}");
                }
            }
        }
    }

    public void SubscribeClient(string username, string channel)
    {
        lock (lockObject)
        {
            if (channel == "IF 100" && !subscribedClientsIF100.Contains(username))
            {
                subscribedClientsIF100.Add(username);
            }
            else if (channel == "SPS 101" && !subscribedClientsSPS101.Contains(username))
            {
                subscribedClientsSPS101.Add(username);
            }
        }
    }
    public List<string> GetSubscribedClientsIF100()
    {
        lock (lockObject)
        {
            return new List<string>(subscribedClientsIF100);
        }
    }

    public List<string> GetSubscribedClientsSPS101()
    {
        lock (lockObject)
        {
            return new List<string>(subscribedClientsSPS101);
        }
    }

    public void UnsubscribeClient(string username, string channel)
    {
        lock (lockObject)
        {
            if (channel == "IF 100")
            {
                subscribedClientsIF100.Remove(username);
            }
            else if (channel == "SPS 101")
            {
                subscribedClientsSPS101.Remove(username);
            }
        }
    }

    public void AddClient(ClientHandler clientHandler)
    {
        lock (lockObject)
        {
            clients.Add(clientHandler);
        }
    }

    public void RemoveClient(ClientHandler clientHandler)
    {
        lock (lockObject)
        {
            clients.Remove(clientHandler);
        }
    }
}

public class ClientHandler
{
    private TcpClient tcpClient;
    private DiSUcordServer server;
    private StreamReader reader;
    private StreamWriter writer;
    public string Username { get; private set; }

    public ClientHandler(DiSUcordServer server, TcpClient tcpClient)
    {
        this.server = server;
        this.tcpClient = tcpClient;
    }

    public void HandleClient()
    {
        try
        {
            NetworkStream networkStream = tcpClient.GetStream();
            reader = new StreamReader(networkStream, Encoding.UTF8);
            writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };

            // Get username from the client
            Username = reader.ReadLine();
            Console.WriteLine($"{Username} connected.");

            // Notify the server about the new client
            server.AddClient(this);

            while (true)
            {
                string clientMessage = reader.ReadLine();
                if (clientMessage == null)
                {
                    break;
                }

                // Process client messages
                string[] messageParts = clientMessage.Split(':');
                if (messageParts.Length == 2)
                {
                    string channel = messageParts[0].Trim();
                    string message = messageParts[1].Trim();

                    // Handle channel subscriptions
                    if (channel == "Subscribe")
                    {
                        server.SubscribeClient(Username, message);
                    }
                    else if (channel == "Unsubscribe")
                    {
                        server.UnsubscribeClient(Username, message);
                    }
                    else
                    {
                        // Broadcast the message to the relevant subscribers
                        List<string> subscribers = GetSubscribers(channel);
                        server.BroadcastMessage(Username, message, subscribers);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            // Cleanup resources and remove the client from the server
            Console.WriteLine($"{Username} disconnected.");
            server.RemoveClient(this);
            tcpClient.Close();
        }
    }

    public void SendMessage(string message)
    {
        writer.WriteLine(message);
    }

    private List<string> GetSubscribers(string channel)
    {
        if (channel == "IF 100")
        {
            return server.GetSubscribedClientsIF100();
        }
        else if (channel == "SPS 101")
        {
            return server.GetSubscribedClientsSPS101();
        }
        else
        {
            return new List<string>();
        }
    }
}

class Program
{
    static void Main()
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); // Change to your server IP address
        int port = 12345; // Change to your desired port number

        DiSUcordServer server = new DiSUcordServer(ipAddress, port);
        server.Start();
    }
}
