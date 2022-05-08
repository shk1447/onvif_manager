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

namespace SaigeVAD.Edge
{
    public class Startup
    {
        private static readonly DateTime unixBase = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        private const string SAIGE_VAD_FPS = "vad/fps";
        private const string SAIGE_VAD_ROI = "vad/roi_xywh";
        private const string SAIGE_MODEL_NAME = "model/name";
        private const string SAIGE_NUM_OF_BATCHES = "vad/num_batches";
        private const string SAIGE_VAD_SCORE_THRESHOLD = "vad/score_threshold";
        private const string SAIGE_VAD_NUM_FRAMES_IN_VCLS_BUFFER = "vad/num_frames_in_vcls_buffer";
        private const string SAIGE_VAD_MASK_TYPE_KEY = "vad/mask_type";
        private const string SAIGE_VAD_MAX_NUM_CONSECUTIVE_ANOMALY_TO_IGNORE = "vad/max_num_consecutive_anomaly_to_ignore";

        private const string BINARY_MASK_TYPE = "Binary";
        private const string NON_BINARY_MASK_TYPE = "Non Binary";
        private const string NA_MASK_TYPE = "N/A";

        private const int NUM_OF_BATCHES = 16;

        static Dictionary<string, WebSocket> sockets = new Dictionary<string, WebSocket>();
        static Dictionary<string, Thread> threads = new Dictionary<string, Thread>();
        static WebSocket ws;
        static InferenceEngine engine;

        public async Task<object> Invoke(dynamic input)
        {

            FFMpegHelper.Register();
            Console.WriteLine("aaaaaaaaaaaaa");

            //var thread = new Thread(ProcessThread);

            //thread.Start();

            try
            {
                var modelPath = Path.Combine(Environment.CurrentDirectory, "./resources/modules/model/Model.svad");
                engine = new InferenceEngine(modelPath, 0);
                Console.WriteLine(engine.ModelInfo);
            }
            catch(Exception err)
            {
                Console.WriteLine(err.ToString());
            }

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
                string url = "rtsp://admin:admin1357@170.101.20.215/stream1";

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

                            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            byte[] byteImage = ms.ToArray();
                            var SigBase64 = Convert.ToBase64String(byteImage);
                            // ws.Send(byteImage);
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

        public int GetStride(int width)
        {
            int bitsPerPixel = this.GetBitsPerPixel();

            return ((width * bitsPerPixel + 31) & ~31) >> 3;
        }

        public int GetBitsPerPixel()
        {
            return 24;
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine("on message");
        }

        private async void Ws_OnOpen(object sender, EventArgs e)
        {            
            Console.WriteLine("on open");

            //var aa = new ConnectionParameters(new Uri("rtsp://170.101.20.216/stream1"), new NetworkCredential("admin", "admin1357"));
            //var test = new RtspClient(aa);
            //test.FrameReceived += Test_FrameReceived;
            //var tokenSource = new CancellationTokenSource();
            //await test.ConnectAsync(tokenSource.Token);
            //await test.ReceiveAsync(tokenSource.Token);

            var thread = new Thread(ProcessThread);

            thread.Start();
        }

        private void Test_FrameReceived(object sender, RawFrame e)
        {
            if(e is RawVideoFrame frame)
            {
                byte[] _extraData = new byte[0];
                unsafe
                {
                    fixed (byte* rawBufferPtr = &frame.FrameSegment.Array[frame.FrameSegment.Offset])
                    {
                        int resultCode;

                        if (frame is RawH264IFrame rawH264IFrame)
                        {
                            if (rawH264IFrame.SpsPpsSegment.Array != null &&
                                !_extraData.SequenceEqual(rawH264IFrame.SpsPpsSegment))
                            {
                                
                            }
                        }
                    }
                }
                Console.WriteLine(frame);
            }
        }
    }
}
