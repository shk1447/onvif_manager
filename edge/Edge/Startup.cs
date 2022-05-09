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

namespace SaigeVAD.Edge
{
    public class Startup
    {
        static int _port = 9090;
        static Dictionary<string, WebSocket> sockets = new Dictionary<string, WebSocket>();
        static Dictionary<string, Thread> threads = new Dictionary<string, Thread>();
        static Dictionary<string, WatsonTcpClient> watsons = new Dictionary<string, WatsonTcpClient>();
        private event EventHandler<MessageReceivedEventArgs> MessageHandler;


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
            var ip = (string)args.ip;
            var port = (int)args.port;
            var ws_url = "ws://127.0.0.1:" + _port + "/edge/async/" + uuid;
            var ws = new WebSocket(ws_url);
            Console.WriteLine("ws url : " + ws_url);

            ws.OnMessage += (object sender, MessageEventArgs e) =>
            {
            };
            ws.OnOpen += (object sender, EventArgs e) =>
            {
                Console.WriteLine("ws open");
                if (!watsons.ContainsKey(ip+port))
                {
                    Console.WriteLine(ip + port);
                    var client = new WatsonTcpClient(ip, port);
                    client.Settings.NoDelay = true;
                    client.Events.ServerConnected += async (object _sender, ConnectionEventArgs _e) =>
                    {
                        Console.WriteLine("connect watson");
                        watsons.Add(ip + port, client);
                        var message = JsonConvert.SerializeObject(new Dictionary<object, object>()
                        {
                            { "result", true }
                        });
                        ws.Send(message);
                    };

                    client.Events.MessageReceived += Events_MessageReceived;

                    client.Events.ServerDisconnected += async (object _sender, DisconnectionEventArgs _e) =>
                    {
                        watsons.Remove(ip + port);
                        var message = JsonConvert.SerializeObject(new Dictionary<object, object>()
                        {
                            { "result", false }
                        });
                        ws.Send(message);
                    };
                    client.Connect();
                } else
                {
                    var message = JsonConvert.SerializeObject(new Dictionary<object, object>()
                    {
                        { "result", true }
                    });
                    ws.Send(message);
                }
            };
            ws.OnClose += (object sender, CloseEventArgs e) =>
            {

            };
            ws.ConnectAsync();
        }

        private void Events_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            MessageHandler.Invoke(sender, e);
        }

        private void SetModel(string uuid, dynamic args)
        {
            Console.WriteLine("set model : " + uuid);
            var ip = (string)args.ip;
            var port = (int)args.port;
            var path = (string)args.path;

            var ws_url = "ws://127.0.0.1:" + _port + "/edge/async/" + uuid;
            var ws = new WebSocket(ws_url);
            Console.WriteLine("ws url : " + ws_url);

            ws.OnMessage += (object sender, MessageEventArgs e) =>
            {
            };
            ws.OnOpen += (object sender, EventArgs e) =>
            {
                Console.WriteLine("ws open : " + uuid);
                var thread = new Thread(() =>
                {
                    Console.WriteLine("thread start : " + path);
                    
                    Console.WriteLine("edge : " + path);
                    try
                    {
                        if(watsons.ContainsKey(ip+port))
                        {
                            var client = watsons[ip + port];

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
                    }
                    catch(Exception err)
                    {
                        Console.WriteLine(err.ToString());
                    }

                });

                sockets.Add(uuid, ws);
                threads.Add(uuid, thread);

                thread.Start();
            };

            ws.OnClose += (object sender, CloseEventArgs e) =>
            {

            };
            ws.ConnectAsync();
        }

        private void StopInference(string uuid, dynamic args)
        {
            Console.WriteLine("test");
        }

        private void StartInference(string uuid, dynamic args)
        {
            Console.WriteLine("start inference");
            var ip = (string)args.ip;
            var port = (int)args.port;
            var rtsp = (dynamic)args.rtsp;

            var ws_url = "ws://127.0.0.1:" + _port + "/edge/async/" + uuid;
            var ws = new WebSocket(ws_url);

            ws.OnMessage += (object sender, MessageEventArgs e) =>
            {
            };
            ws.OnOpen += (object sender, EventArgs e) =>
            {
                Console.WriteLine("ws open");
                const byte H264IFRAME_CODE = 5;
                const byte H264PFRAME_CODE = 7;
                var thread = new Thread(async () =>
                {
                    try
                    {
                        if(watsons.ContainsKey(ip + port))
                        {
                            var client = watsons[ip + port];
                            MessageHandler += (object _sender, MessageReceivedEventArgs _e) =>
                            {
                                Console.WriteLine(_e.Metadata);
                            };

                            var _metadata = new Dictionary<object, object>();
                            _metadata.Add("Request", "Start");
                            SyncResponse _response = client.SendAndWait(60_000, string.Empty, _metadata);

                            var conn_params = new ConnectionParameters(new Uri(rtsp.url), new NetworkCredential(rtsp.username, rtsp.password));
                            var rtspClient = new RtspClient(conn_params);
                            rtspClient.FrameReceived += (object __sender, RawFrame frame) =>
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
                                        Console.WriteLine(buffer.Length);
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
                                        Console.WriteLine(buffer.Length);
                                        client.Send(buffer, metadata);
                                    }

                                }
                            };
                            var tokenSource = new CancellationTokenSource();
                            await rtspClient.ConnectAsync(tokenSource.Token);
                            await rtspClient.ReceiveAsync(tokenSource.Token);
                            Console.WriteLine("thread inference");
                        }
                    }
                    catch(Exception err)
                    {
                        Console.WriteLine(err.ToString());
                    }
                });

                sockets.Add(uuid, ws);
                threads.Add(uuid, thread);

                thread.Start();
            };

            ws.OnClose += (object sender, CloseEventArgs e) =>
            {

            };
            ws.ConnectAsync();
        }

        private unsafe void StreamVideo(string uuid, dynamic args)
        {
            var url = (string)args.url;
            var ws = new WebSocket("ws://127.0.0.1:"+_port+"/edge/async/"+uuid);

            ws.OnMessage += (object sender, MessageEventArgs e) =>
            {
            };
            ws.OnOpen += (object sender, EventArgs e) =>
            {
                var thread = new Thread(() =>
                {
                    Console.WriteLine("start decoding process");
                    try
                    {
                        using (VideoStreamDecoder decoder = new VideoStreamDecoder(url))
                        {
                            IReadOnlyDictionary<string, string> contextInfoDictionary = decoder.GetContextInfoDictionary();

                            contextInfoDictionary.ToList().ForEach(x => Console.WriteLine($"{x.Key} = {x.Value}"));

                            Size sourceSize = decoder.FrameSize;
                            AVPixelFormat sourcePixelFormat = decoder.PixelFormat;
                            Size targetSize = sourceSize;
                            AVPixelFormat targetPixelFormat = AVPixelFormat.AV_PIX_FMT_BGR24;
                            using (VideoFrameConverter converter = new VideoFrameConverter(sourceSize, sourcePixelFormat, targetSize, targetPixelFormat))
                            {
                                int frameNumber = 0;

                                while (decoder.TryDecodeNextFrame(out AVFrame sourceFrame) && true)
                                {
                                    MemoryStream ms = new MemoryStream();
                                    AVFrame targetFrame = converter.Convert(sourceFrame);

                                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap
                                    (
                                        targetFrame.width,
                                        targetFrame.height,
                                        targetFrame.linesize[0],
                                        System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                                        (IntPtr)targetFrame.data[0]
                                    );

                                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                    byte[] byteImage = ms.ToArray();
                                    var SigBase64 = Convert.ToBase64String(byteImage);
                                    Dictionary<string, string> data = new Dictionary<string, string>();
                                    data.Add("stream", SigBase64);
                                    var message = JsonConvert.SerializeObject(data);
                                    ws.Send(message);
                                    frameNumber++;
                                }
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err.ToString());
                    }
                });
                sockets.Add(uuid, ws);
                threads.Add(uuid, thread);

                thread.Start();
            };
            ws.OnClose += (object sender, CloseEventArgs e) =>
            {
                
            };
            ws.ConnectAsync();
        }

        private void ThreadExit(string uuid)
        {
            if(threads.ContainsKey(uuid))
            {
                sockets[uuid].Close();
                threads[uuid].Abort();
            }
        }
    }
}
