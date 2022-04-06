using System;
using System.Collections.Generic;
using System.Text;
using WatsonTcp;

namespace TcpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            WatsonTcpClient client = new WatsonTcpClient("127.0.0.1", 9000);
            client.Events.ServerConnected += ServerConnected;
            client.Events.ServerDisconnected += ServerDisconnected;
            client.Events.MessageReceived += MessageReceived;
            client.Callbacks.SyncRequestReceived = SyncRequestReceived;
            client.Connect();

            // check connectivity
            Console.WriteLine("Am I connected?  " + client.Connected);

            // send a message
            client.Send("Hello!");

            // send a message with metadata
            Dictionary<object, object> md = new Dictionary<object, object>();
            md.Add("foo", "bar");
            client.Send("Hello, client!  Here's some metadata!", md);

            // send async!
            // await client.SendAsync("Hello, client!  I'm async!");

            // send and wait for a response
            try
            {
                SyncResponse resp = client.SendAndWait(5000, "Hey, say hello back within 5 seconds!");
                Console.WriteLine("My friend says: " + Encoding.UTF8.GetString(resp.Data));
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Too slow...");
            }

            Console.ReadLine();
        }

        static void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Console.WriteLine(args.Metadata.Count);
            Console.WriteLine("Message from " + args.IpPort + ": " + Encoding.UTF8.GetString(args.Data));
        }

        static void ServerConnected(object sender, ConnectionEventArgs args)
        {
            Console.WriteLine("Server " + args.IpPort + " connected");
        }

        static void ServerDisconnected(object sender, DisconnectionEventArgs args)
        {
            Console.WriteLine("Server " + args.IpPort + " disconnected");
        }

        static SyncResponse SyncRequestReceived(SyncRequest req)
        {
            return new SyncResponse(req, "Hello back at you!");
        }
    }
}
