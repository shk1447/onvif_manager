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

namespace SaigeVAD.Edge
{
    public class Startup
    {
        static Dictionary<string, WebSocket> sockets = new Dictionary<string, WebSocket>();
        static WebSocket ws;

        public async Task<object> Invoke(dynamic input)
        {

            FFMpegHelper.Register();
            Console.WriteLine("aaaaaaaaaaaaa");
            //var aa = new ConnectionParameters(new Uri("rtsp://170.101.20.216/stream1"), new NetworkCredential("admin", "admin1357"));
            //var test = new RtspClient(aa);
            //test.FrameReceived += Test_FrameReceived;
            //var tokenSource = new CancellationTokenSource();
            //await test.ConnectAsync(tokenSource.Token);
            //await test.ReceiveAsync(tokenSource.Token);

            //var thread = new Thread(ProcessThread);

            //thread.Start();

            ws = new WebSocket("ws://127.0.0.1:9090/rtsp/image");

            ws.OnMessage += Ws_OnMessage;
            ws.OnOpen += Ws_OnOpen;
            ws.ConnectAsync();

            return true;
        }

        private unsafe void ProcessThread()
        {
            Console.WriteLine("start decoding process");
            try
            {
                string url = "rtsp://admin:admin1357@170.101.20.216/stream1";

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

                            System.Drawing.Bitmap bitmap;

                            bitmap = new System.Drawing.Bitmap
                            (
                                targetFrame.width,
                                targetFrame.height,
                                targetFrame.linesize[0],
                                System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                                (IntPtr)targetFrame.data[0]
                            );

                            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            byte[] byteImage = ms.ToArray();
                            // var SigBase64 = Convert.ToBase64String(byteImage);
                            ws.Send(byteImage);

                            frameNumber++;
                        }
                    }
                }
            }
            catch(Exception err)
            {
                Console.WriteLine(err.ToString());
            }
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine("on message");
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {            
            Console.WriteLine("on open");

            var thread = new Thread(ProcessThread);

            thread.Start();
        }

    }
}
