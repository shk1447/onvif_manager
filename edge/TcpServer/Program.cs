using System;
using System.Collections.Generic;
using System.Text;
using WatsonTcp;

namespace TcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            WatsonTcpServer server = new WatsonTcpServer("127.0.0.1", 9000);
            server.Events.ClientConnected += ClientConnected;
            server.Events.ClientDisconnected += ClientDisconnected;
            server.Events.MessageReceived += MessageReceived;
            server.Callbacks.SyncRequestReceived = SyncRequestReceived;
            server.Start();

            // list clients
            IEnumerable<string> clients = server.ListClients();

            // send a message
            server.Send("[IP:port]", "Hello, client!");

            // send async!
            // await server.SendAsync("[IP:port", "Hello, client!  I'm async!");

            // send and wait for a response
            try
            {
                // SyncResponse resp = server.SendAndWait(5000, "Hey, say hello back within 5 seconds!");
                // Console.WriteLine("My friend says: " + Encoding.UTF8.GetString(resp.Data));
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Too slow...");
            }

            Console.ReadLine();
        }

        static void ClientConnected(object sender, ConnectionEventArgs args)
        {
            Console.WriteLine("Client connected: " + args.IpPort);

            var server = sender as WatsonTcpServer;

            // send Async
            Dictionary<object, object> md = new Dictionary<object, object>();
            md.Add("foo", "bar");
            server.Send(args.IpPort, "Hello, client!  Here's some metadata!", md);

            // send Sync
            SyncResponse resp = server.SendAndWait(5000, args.IpPort, "Hey, say hello back within 5 seconds!");
            Console.WriteLine(resp.Data);
        }

        static void ClientDisconnected(object sender, DisconnectionEventArgs args)
        {
            Console.WriteLine("Client disconnected: " + args.IpPort + ": " + args.Reason.ToString());
        }

        static void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Console.WriteLine("Message from " + args.IpPort + ": " + Encoding.UTF8.GetString(args.Data));
        }

        static SyncResponse SyncRequestReceived(SyncRequest req)
        {
            return new SyncResponse(req, "Hello back at you!");
        }
    }
}
