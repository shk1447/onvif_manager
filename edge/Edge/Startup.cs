using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using WebSocketSharp;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using FFmpeg.AutoGen;
using System.IO;
using SaigeVAD.Edge.Engine.PlatformInvoke;
using SaigeVAD.Edge.Engine.Inspector;
using SaigeVAD.Edge.Engine;
using SaigeVAD.Edge.Engine.Common;
using RtspClientSharp;
using RtspClientSharp.RawFrames;
using RtspClientSharp.RawFrames.Video;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using WatsonTcp;
using System.Text;

namespace SaigeVAD.Edge
{
    public class Startup
    {
        static int _port = 9090;
        static Dictionary<string, WebSocket> sockets = new Dictionary<string, WebSocket>();
        static Dictionary<string, dynamic> shared = new Dictionary<string, dynamic>();


        public async Task<object> Invoke(dynamic input)
        {
            string uuid = (string)input.uuid;
            string method = (string)input.method;
            dynamic args = (dynamic)input.args;

            switch(method)
            {
                case "Initialize":
                    this.Initialize(args);
                    break;
                case "ConnectEngine":
                    this.ConnectEngine(uuid, args);
                    break;
                case "SetModel":
                    this.SetModel(uuid, args);
                    break;
                case "StartInference":
                    this.StartInference(uuid, args);
                    break;
                case "StopInference":
                    this.StopInference(uuid, args);
                    break;
                case "ThreadExit":
                    this.ThreadExit(uuid);
                    break;
            }

            return uuid;
        }

        private void Initialize(dynamic args)
        {
            int port = (int)args.port;
            _port = port;
            FFMpegHelper.Register();
            Console.WriteLine("initialize");
        }

        private void ConnectEngine(string uuid, dynamic args)
        {
            if (sockets.ContainsKey(uuid)) return;

            var ip = (string)args.ip;
            var port = (int)args.port;
            var ws_url = "ws://127.0.0.1:" + _port + "/edge/async/" + uuid;
            var ws = new WebSocket(ws_url);

            WatsonTcpClient client = default;
            EventHandler<ConnectionEventArgs> connectedHandler = async (object sender, ConnectionEventArgs e) =>
            {
                if(ws.IsAlive)
                {
                    var message = JsonConvert.SerializeObject(new Dictionary<object, object>()
                        {
                            { "result", true }
                        });
                    ws.Send(message);
                }
            };
            EventHandler<DisconnectionEventArgs> disconnectHandler = async (object sender, DisconnectionEventArgs e) =>
            {
                var message = JsonConvert.SerializeObject(new Dictionary<object, object>()
                        {
                            { "result", false }
                        });
                ws.Send(message);
            };
            EventHandler<MessageReceivedEventArgs> messageHandler = async (object sender, MessageReceivedEventArgs e) =>
            {
                if (e.Metadata.TryGetValue("Response", out object value))
                {
                    string requestName = (string)value;
                    Console.WriteLine(requestName + uuid);
                    switch (requestName)
                    {
                        case "frameAnalysed":
                            string data = Encoding.Default.GetString(e.Data);
                            try
                            {
                                ws.Send(data);
                            }
                            catch(Exception err)
                            {
                                Console.WriteLine(err.ToString());
                            }
                            
                            break;

                        case "freeRoi":

                            break;
                    }
                }
            };

            EventHandler openHandler = default;
            EventHandler<CloseEventArgs> closeHandler = default;

            openHandler = (object sender, EventArgs e) =>
            {
                client = new WatsonTcpClient(ip, port);

                sockets.Add(uuid, ws);
                shared.Add(uuid, client);

                client.Settings.NoDelay = true;

                client.Events.ServerConnected += connectedHandler;
                client.Events.MessageReceived += messageHandler;
                client.Events.ServerDisconnected += disconnectHandler;

                client.Connect();

                var message = JsonConvert.SerializeObject(new Dictionary<object, object>()
                {
                    { "result", true }
                });
                ws.Send(message);
            };

            closeHandler = (object sender, CloseEventArgs e) =>
            {
                client.Events.ServerConnected -= connectedHandler;
                client.Events.MessageReceived -= messageHandler;
                client.Events.ServerDisconnected -= disconnectHandler;
                ws.OnOpen -= openHandler;
                ws.OnClose -= closeHandler;

                client.Disconnect();
                client.Dispose();
                sockets.Remove(uuid);
                shared.Remove(uuid);
            };
            ws.OnOpen += openHandler;
            ws.OnClose += closeHandler;

            ws.ConnectAsync();
        }

        private void SetModel(string uuid, dynamic args)
        {
            if (sockets.ContainsKey(uuid)) return;

            var path = (string)args.path;
            var shared_id = (string)args.shared_id;

            var ws_url = "ws://127.0.0.1:" + _port + "/edge/async/" + uuid;
            var ws = new WebSocket(ws_url);
            EventHandler openHandler = default;
            EventHandler<CloseEventArgs> closeHandler = default;
            openHandler = (object sender, EventArgs e) =>
            {
                sockets.Add(uuid, ws);
                var client = shared[shared_id] as WatsonTcpClient;
                try
                {
                    var metadata = new Dictionary<object, object>();
                    metadata.Add("Request", "Initialization");
                    metadata.Add("newModel", true);
                    using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        SyncResponse response = client.SendAndWait(60_000, stream.Length, stream, metadata);
                        Dictionary<object, object> data = new Dictionary<object, object>();

                        data.Add("result", response.Data[0]);
                        var message = JsonConvert.SerializeObject(data);
                        ws.Send(message);
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.ToString());
                }
            };

            closeHandler = (object sender, CloseEventArgs e) =>
            {
                ws.OnOpen -= openHandler;
                ws.OnClose -= closeHandler;
                sockets.Remove(uuid);
            };

            ws.OnOpen += openHandler;
            ws.OnClose += closeHandler;

            ws.ConnectAsync();
        }

        private void StopInference(string uuid, dynamic args)
        {
            Console.WriteLine("test");
        }

        private void StartInference(string uuid, dynamic args)
        {
            if (sockets.ContainsKey(uuid)) return;

            var ip = (string)args.ip;
            var port = (int)args.port;
            var rtsp = (dynamic)args.rtsp;
            var shared_id = (string)args.shared_id;
            var shared_list = shared_id.Split(',');

            var ws_url = "ws://127.0.0.1:" + _port + "/edge/async/" + uuid;
            var ws = new WebSocket(ws_url);
            RtspClient rtspClient = default;
            EventHandler openHandler = default;
            EventHandler<CloseEventArgs> closeHandler = default;
            Thread thread = default;
            CancellationTokenSource tokenSource = default;
            var client = shared[shared_id] as WatsonTcpClient;
            const byte H264IFRAME_CODE = 5;
            const byte H264PFRAME_CODE = 7;
            EventHandler<RawFrame> frameReceivedHandler = (object __sender, RawFrame frame) =>
            {
                try
                {
                    if (frame is RawH264Frame h264frame)
                    {
                        int bufferLen;
                        byte[] buffer;

                        if (h264frame is RawH264IFrame iframe)
                        {
                            bufferLen = 1 + 4 + 4 + iframe.FrameSegment.Count + iframe.SpsPpsSegment.Count + sizeof(long);
                            buffer = new byte[bufferLen];
                            buffer[0] = H264IFRAME_CODE;
                            byte[] tickBytes = BitConverter.GetBytes(iframe.Timestamp.Ticks);
                            tickBytes.CopyTo(buffer, 1);
                            byte[] fsBytes = BitConverter.GetBytes(iframe.FrameSegment.Count);
                            fsBytes.CopyTo(buffer, 9);
                            byte[] spsBytes = BitConverter.GetBytes(iframe.SpsPpsSegment.Count);
                            spsBytes.CopyTo(buffer, 13);

                            var source = iframe.FrameSegment;
                            Buffer.BlockCopy(source.Array, source.Offset, buffer, 17, source.Count);
                            source = iframe.SpsPpsSegment;
                            var startToWrite = 17 + iframe.FrameSegment.Count;
                            Buffer.BlockCopy(source.Array, source.Offset, buffer, startToWrite, source.Count);
                            var metadata = new Dictionary<object, object>(5);
                            metadata.Add("Request", "NextFrame");
                            metadata.Add("type", string.Empty);
                            metadata.Add("seg1", string.Empty);
                            metadata.Add("seg2", string.Empty);
                            metadata.Add("en", "h264");
                            client.Send(buffer, metadata);
                        }
                        else if (h264frame is RawH264PFrame pframe)
                        {
                            bufferLen = 1 + 4 + frame.FrameSegment.Count + sizeof(long);
                            buffer = new byte[bufferLen];
                            buffer[0] = H264PFRAME_CODE;
                            byte[] tickBytes = BitConverter.GetBytes(pframe.Timestamp.Ticks);
                            tickBytes.CopyTo(buffer, 1);
                            byte[] fsBytes = BitConverter.GetBytes(pframe.FrameSegment.Count);
                            fsBytes.CopyTo(buffer, 9);

                            var source = pframe.FrameSegment;
                            Buffer.BlockCopy(source.Array, source.Offset, buffer, 13, source.Count);

                            var metadata = new Dictionary<object, object>(5);
                            metadata.Add("Request", "NextFrame");
                            metadata.Add("type", string.Empty);
                            metadata.Add("seg1", string.Empty);
                            metadata.Add("seg2", string.Empty);
                            metadata.Add("en", "h264");
                            client.Send(buffer, metadata);
                        }
                    }
                }
                catch(Exception err)
                {
                    Console.WriteLine("Right!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + err.ToString());
                }
            };

            openHandler = async (object sender, EventArgs e) =>
            {
                var _metadata = new Dictionary<object, object>();
                _metadata.Add("Request", "Start");
                SyncResponse _response = client.SendAndWait(60_000, string.Empty, _metadata);
                var conn_params = new ConnectionParameters(new Uri(rtsp.url), new NetworkCredential(rtsp.username, rtsp.password));
                rtspClient = new RtspClient(conn_params);
                sockets.Add(uuid, ws);
                shared.Add(uuid, rtspClient);
                rtspClient.FrameReceived += frameReceivedHandler;
                tokenSource = new CancellationTokenSource();
                await rtspClient.ConnectAsync(tokenSource.Token);
                await rtspClient.ReceiveAsync(tokenSource.Token);                
            };

            closeHandler = (object sender, CloseEventArgs e) =>
            {
                Console.WriteLine(uuid + " : Closed!?!?!?");
                try
                {
                    ws.OnOpen -= openHandler;
                    Console.WriteLine("open handler");
                    ws.OnClose -= closeHandler;
                    Console.WriteLine("close handler");
                    Console.WriteLine("dispose");
                    rtspClient.FrameReceived -= frameReceivedHandler;

                    Console.WriteLine("receive handler");
                    Console.WriteLine("completed");
                    sockets.Remove(uuid);
                    shared.Remove(uuid);
                } catch(Exception err)
                {
                    Console.WriteLine("aaaaaaaaaaa"+err.ToString());
                }
            };

            ws.OnOpen += openHandler;
            ws.OnClose += closeHandler;

            ws.ConnectAsync();
        }

        private void ThreadExit(string uuid)
        {
            
            if(sockets.ContainsKey(uuid))
            {
                sockets[uuid].Close();
                Console.WriteLine(uuid + " : Close");
            } else
            {
                Console.WriteLine(uuid + " : WHy not!?!?!?");
            }
        }
    }
}
