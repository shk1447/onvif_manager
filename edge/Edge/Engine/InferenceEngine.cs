using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaigeVAD.Edge.Engine
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;
    using SaigeVAD.Edge.Engine.Common;
    using SaigeVAD.Edge.Engine.PlatformInvoke;
    using SaigeVAD.Edge.Engine.Inspector;

    public class MaskInfo
    {
        public Rectangle Roi { get; set; }
        public byte[] Contour { get; set; }
    }

    public class InferenceModelInfo
    {
        public string Id { get; }

        public MaskInfo Roi { get; }

        public int Fps { get; }

        public string Name { get; }

        public string MaskType { get; }

        public double Threshold { get; private set; }

        public int VclsBuffer { get; private set; }

        public int NumOfFramesToDrop { get; private set; }

        public InferenceModelInfo(
            string id, MaskInfo roi, int fps, string name,
            double threshold, int vclsBuffer, string maskType, int numOfFramesToDrop)
        {
            this.Id = id;
            this.Roi = roi;
            this.Fps = fps;
            this.Name = name;
            this.Threshold = threshold;
            this.VclsBuffer = vclsBuffer;
            this.MaskType = maskType;
            this.NumOfFramesToDrop = numOfFramesToDrop;
        }

        public void ResetThreshold(double threshold)
        {
            this.Threshold = threshold;
        }

        public void ResetVclsBuffer(int vclsBuffer)
        {
            this.VclsBuffer = vclsBuffer;
        }

        public void ResetNumOfFramesToDrop(int numOfFramesToDrop)
        {
            this.NumOfFramesToDrop = numOfFramesToDrop;
        }
    }

    public class InferenceEngine : IDisposable
    {
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

        public InferenceModelInfo ModelInfo { get; private set; }

        public InferenceEngine(string modelPath, int deviceIndex)
        {
            string id = Guid.NewGuid().ToString();
            var modelBuffer = default(ModelBuffer);

            ResponseCode response = NativeVadInspector.InitModel(modelPath, id);
            Console.WriteLine("init model"+response);

            response = NativeVadInspector.InitBuffer(ref modelBuffer);
            Console.WriteLine("init buffer" + response);

            string name = this.ExtractModelName(id, ref modelBuffer);
            Console.WriteLine("extract name" + name);
            int fps = this.ExtractFps(id, ref modelBuffer);
            Console.WriteLine("extract fps" + fps);
            Rectangle roi = this.ExtractRoi(id, ref modelBuffer);
            Console.WriteLine("extract roi" + roi.Width + "/" + roi.Height);
            double threshold = this.ExtractThreshold(id, ref modelBuffer);
            Console.WriteLine("extract threshold" + threshold);
            int vclsBuffer = this.ExtractVclsBuffer(id, ref modelBuffer);
            Console.WriteLine("extract vcls" + vclsBuffer);
            string maskType = this.ExtractModelMaskType(id, ref modelBuffer);
            Console.WriteLine("extract mask" + maskType);
            int numOfFramesToDrop = this.ExtractNumOfFramesToDrop(id, ref modelBuffer);
            Console.WriteLine("extract drop frame" + numOfFramesToDrop);

            NativeVadInspector.DeleteBuffer(ref modelBuffer);
            Console.WriteLine("delete buffer");

            response = NativeVadInspector.SetModelParameter(id, SAIGE_NUM_OF_BATCHES, NUM_OF_BATCHES.ToString());

            Console.WriteLine("set model params" + response);


            response = NativeVadInspector.SetDevice(id, DeviceType.CPU, 0);
            Console.WriteLine("set device" + response);

            this.ModelInfo = new InferenceModelInfo(
                                    id, new MaskInfo { Roi = roi }, fps, name, threshold, vclsBuffer,
                                    maskType, numOfFramesToDrop);

            Console.WriteLine("complete!!!!");
        }

        [SuppressMessage("Design", "CA1021:out 매개 변수를 사용하지 마십시오.", Justification = "문제없습니다.")]
        public ResponseCode Inference(
            byte[] rawData, Size size, int channels, SaigeDepth depth, long timeStamp, out ImageResults[] results)
        {
            var srVadImage = default(SrVADImage);
            uint numOfImageResult = 0;

            var res01 = NativeVadInspector.CreateImageFromBlob(
                size.Width, size.Height, channels, depth, ImageDataFormat.HWC, rawData, ref srVadImage);
            Console.WriteLine("create blob");
            Console.WriteLine(res01);
            var res = NativeVadInspector.PushImage(this.ModelInfo.Id, ref srVadImage, timeStamp, ref numOfImageResult);
            Console.WriteLine(res);
            Console.WriteLine("push image");

            (ResponseCode, ImageResults[]) imageResults = ExtractImageResult(numOfImageResult);
            Console.WriteLine("extract image");

            results = imageResults.Item2;
            return imageResults.Item1;
        }

        public void SetRoiContour(byte[] contour)
        {
            InferenceModelInfo modelInfo = this.ModelInfo;
            this.ModelInfo = new InferenceModelInfo(modelInfo.Id,
                                                        new MaskInfo { Roi = modelInfo.Roi.Roi, Contour = contour },
                                                        modelInfo.Fps,
                                                        modelInfo.Name,
                                                        modelInfo.Threshold,
                                                        modelInfo.VclsBuffer,
                                                        modelInfo.MaskType,
                                                        modelInfo.NumOfFramesToDrop);
        }

        public void SaveMaskContoursInJson(string path)
        {
            NativeVadInspector.SaveMaskContoursInJson(this.ModelInfo.Id, path);
        }

        public void Reset()
        {
            NativeVadInspector.Reset(this.ModelInfo.Id);
        }

        public void ResetThreshold(double threshold)
        {
            NativeVadInspector.SetModelParameter(this.ModelInfo.Id, SAIGE_VAD_SCORE_THRESHOLD, threshold.ToString());
            UpdateModel();
        }

        public void ResetVclsBuffer(int vclsBuffer)
        {
            NativeVadInspector.SetModelParameter(this.ModelInfo.Id, SAIGE_VAD_NUM_FRAMES_IN_VCLS_BUFFER, vclsBuffer.ToString());
            UpdateModel();
        }

        public void ResetNumOfFramesToDrop(int numOfFramesToDrop)
        {
            NativeVadInspector.SetModelParameter(this.ModelInfo.Id, SAIGE_VAD_MAX_NUM_CONSECUTIVE_ANOMALY_TO_IGNORE, numOfFramesToDrop.ToString());
            UpdateModel();
        }

        /// <summary>
        /// 검사 중 엔진 parameter값이 바뀌면 그 값을 모델파일에 업데이트 해줍니다.
        /// </summary>
        private void UpdateModel()
        {
            NativeVadInspector.OverwriteParameters(this.ModelInfo.Id);
        }

        private (ResponseCode, ImageResults[]) ExtractImageResult(uint numOfImageResult)
        {
            var results = new ImageResults[numOfImageResult];

            if (numOfImageResult < 1)
                return (ResponseCode.SUCCESS, results);

            var srVadResultArray = new SrVADResult[numOfImageResult];
            NativeVadInspector.RetrieveResults(this.ModelInfo.Id, numOfImageResult, srVadResultArray);

            bool isInvalidResult = srVadResultArray.Any(q => q.ErrorCode != ResponseCode.SUCCESS);
            if (isInvalidResult)
                return (ResponseCode.UNKNOWN, results);

            for (int i = 0; i < numOfImageResult; i++)
            {
                results[i] = new ImageResults
                {
                    TimeStamp = srVadResultArray[i].TimeStamp,
                    NormalizedScore = srVadResultArray[i].NormalizedScore,
                    Threshold = srVadResultArray[i].Threshold,
                    BaseScore = srVadResultArray[i].BaseScore,
                    AdaptiveScore = srVadResultArray[i].AdaptiveScore,
                    BaseThreshold = srVadResultArray[i].BaseThreshold,
                    SlowThreshold = srVadResultArray[i].SlowThreshold,
                    SlowMean = srVadResultArray[i].SlowMean,
                    SlowVar = srVadResultArray[i].SlowVar,
                    FastThreshold = srVadResultArray[i].FastThreshold,
                    FastMean = srVadResultArray[i].FastMean,
                    FastVar = srVadResultArray[i].FastVar,
                    ClusterIndex = srVadResultArray[i].ClusterIndex,
                    AdaptationStep = srVadResultArray[i].AdaptationStep,
                    FreezeThresholdCount = srVadResultArray[i].FreezeThresholdCount,
                    HardExampleMiningScore = srVadResultArray[i].HardExampleMiningScore,
                    IsNormal = Convert.ToBoolean(srVadResultArray[i].IsNormal),
                    IsMotion = Convert.ToBoolean(srVadResultArray[i].IsMotion),
                    IsOverwrittenByVcls = Convert.ToBoolean(srVadResultArray[i].IsOverwrittenByVcls),
                    VclsClassIndex = srVadResultArray[i].VclsClassIndex,
                    PhaseIndex = srVadResultArray[i].PhaseIndex,
                    IsInspected = Convert.ToBoolean(srVadResultArray[i].IsInspected),
                    Roi = new Rectangle(srVadResultArray[i].Roi_X, srVadResultArray[i].Roi_Y, srVadResultArray[i].Roi_W, srVadResultArray[i].Roi_H),
                    ErrorCode = srVadResultArray[i].ErrorCode
                };
            }

            return (ResponseCode.SUCCESS, results);
        }

        private string ExtractModelMaskType(string id, ref ModelBuffer buffer)
        {
            NativeVadInspector.GetModelParameter(id, SAIGE_VAD_MASK_TYPE_KEY, ref buffer);
            ValidateEngineBuffer(SAIGE_VAD_MASK_TYPE_KEY, ref buffer);

            int maskType = Marshal.PtrToStructure<int>(buffer.Data);

            return ConversionMaskTypeToString(maskType);
        }

        private string ConversionMaskTypeToString(int maskType)
        {
            string ret = "EMPTY";
            switch (maskType)
            {
                case (int)MaskType.NA:
                    ret = NA_MASK_TYPE;
                    break;

                case (int)MaskType.Binary:
                    ret = BINARY_MASK_TYPE;
                    break;

                case (int)MaskType.Non_Binary:
                    ret = NON_BINARY_MASK_TYPE;
                    break;
            }
            return ret;
        }

        private string ExtractModelName(string id, ref ModelBuffer buffer)
        {
            NativeVadInspector.GetModelParameter(id, SAIGE_MODEL_NAME, ref buffer);

            string name = Marshal.PtrToStringAnsi(buffer.Data);

            return name;
        }

        private int ExtractFps(string id, ref ModelBuffer buffer)
        {
            NativeVadInspector.GetModelParameter(id, SAIGE_VAD_FPS, ref buffer);
            ValidateEngineBuffer(SAIGE_VAD_FPS, ref buffer);

            int fps = Marshal.PtrToStructure<int>(buffer.Data);

            return fps;
        }

        private Rectangle ExtractRoi(string id, ref ModelBuffer buffer)
        {
            NativeVadInspector.GetModelParameter(id, SAIGE_VAD_ROI, ref buffer);
            ValidateEngineBuffer(SAIGE_VAD_ROI, ref buffer);

            int[] roiElements = new int[buffer.Length];
            Marshal.Copy(buffer.Data, roiElements, 0, buffer.Length);

            var roi = new Rectangle(
                    x: roiElements[0], y: roiElements[1], width: roiElements[2], height: roiElements[3]);

            return roi;
        }

        private double ExtractThreshold(string id, ref ModelBuffer buffer)
        {
            NativeVadInspector.GetModelParameter(id, SAIGE_VAD_SCORE_THRESHOLD, ref buffer);
            ValidateEngineBuffer(SAIGE_VAD_SCORE_THRESHOLD, ref buffer);

            double threshold = Marshal.PtrToStructure<double>(buffer.Data);

            return threshold;
        }

        private int ExtractVclsBuffer(string id, ref ModelBuffer buffer)
        {
            NativeVadInspector.GetModelParameter(id, SAIGE_VAD_NUM_FRAMES_IN_VCLS_BUFFER, ref buffer);
            ValidateEngineBuffer(SAIGE_VAD_NUM_FRAMES_IN_VCLS_BUFFER, ref buffer);

            int vclsBuffer = Marshal.PtrToStructure<int>(buffer.Data);

            return vclsBuffer;
        }

        private int ExtractNumOfFramesToDrop(string id, ref ModelBuffer buffer)
        {
            NativeVadInspector.GetModelParameter(id, SAIGE_VAD_MAX_NUM_CONSECUTIVE_ANOMALY_TO_IGNORE, ref buffer);
            ValidateEngineBuffer(SAIGE_VAD_MAX_NUM_CONSECUTIVE_ANOMALY_TO_IGNORE, ref buffer);

            int numOfFramesToDrop = Marshal.PtrToStructure<int>(buffer.Data);

            return numOfFramesToDrop;
        }

        private void ValidateEngineBuffer(string modelParam, ref ModelBuffer modelBuffer)
        {
            switch (modelParam)
            {
                case SAIGE_VAD_FPS:
                    if (modelBuffer.Length != 1 || modelBuffer.Type != BufferType.UINT)
                        throw new Exception();
                    break;

                case SAIGE_VAD_ROI:
                    if (modelBuffer.Length != 4 || modelBuffer.Type != BufferType.INT)
                        throw new Exception();
                    break;

                case SAIGE_VAD_SCORE_THRESHOLD:
                    if (modelBuffer.Length != 1 || modelBuffer.Type != BufferType.DOUBLE)
                        throw new Exception();
                    break;

                case SAIGE_VAD_NUM_FRAMES_IN_VCLS_BUFFER:
                    if (modelBuffer.Length != 1 || modelBuffer.Type != BufferType.UINT)
                        throw new Exception();
                    break;

                case SAIGE_VAD_MAX_NUM_CONSECUTIVE_ANOMALY_TO_IGNORE:
                    if (modelBuffer.Length != 1 || modelBuffer.Type != BufferType.UINT)
                        throw new Exception();
                    break;

                case SAIGE_VAD_MASK_TYPE_KEY:
                    if (modelBuffer.Length != 1 || modelBuffer.Type != BufferType.INT)
                        throw new Exception();
                    break;

                default:
                    break;
            }
        }

        private void ThrowIfitFailed(ResponseCode code, ref ModelBuffer modelBuffer, string id)
        {
            if (code != ResponseCode.SUCCESS)
            {
                NativeVadInspector.DeleteBuffer(ref modelBuffer);
                NativeVadInspector.DeleteModel(id);

                throw new Exception(code.ToString());
            }
        }

        public void Dispose()
        {
            UpdateModel();
            NativeVadInspector.DeleteModel(this.ModelInfo.Id);
            this.ModelInfo = null;
        }
    }

    internal enum MaskType
    {
        NA,
        Binary,
        Non_Binary
    }
}
