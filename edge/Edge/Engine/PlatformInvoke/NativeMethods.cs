namespace SaigeVAD.Edge.Engine.PlatformInvoke
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [SuppressMessage("Globalization", "CA2101:P/Invoke 문자열 인수에 대해 마샬링을 지정하세요.",
                                    Justification = "기본 사항을 유지합니다.")]

    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1516:Elements should be separated by blank line",
                                    Justification = "관련성있는 함수 연결에 필요합니다.")]

    [SuppressMessage("Security", "CA5393:안전하지 않은 DllImportSearchPath 값 사용 안 함",
                                    Justification = "문제 없습니다.")]

    [SuppressMessage("Design", "CA1060:PInvoke를 네이티브 메서드 클래스로 이동하세요.",
                                    Justification = "Class로 분류 필요합니다.")]

    internal class NativeMethods
    {
        public const string SAIGE_DLL_NAME = "saige195.dll";
        public const string VAD_DLL_NAME = "saige_vad195.dll";
        public const string SAIGE_LICENSE_DLL_NAME = "saige_license195.dll";

        #region ModelApi

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_InitModel(string path, string id);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_GetNumClasses(string id, ref uint numOfClasses);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_GetSingleClassInfo(string id, int index, ref ModelBuffer param, ref int color);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_GetModelParameter(string id, string paramName, ref ModelBuffer param);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_GetModelParameterWithIndex(string id, string paramName, int index, IntPtr param);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_GetSubModelParameter(string id, string subModelName, string paramName, IntPtr param);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_GetSubModelParameterWithIndex(string modidelName, string subModelName, string paramName, int index, IntPtr param);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_SetModelParameter(string id, string paramName, string value);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_SetModelParameterWithIndex(string id, string paramName, int index, string value);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_SetSubModelParameter(string id, string subModelName, string paramName, string value);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_SetSubModelParameterWithIndex(string id, string subModelName, string paramName, int index, string value);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_SetDevice(string id, DeviceType deviceType, int deviceIndex);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_SaveTorchFile(string id, string parentPath);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_PushImage(string id, ref SrVADImage image, long stamp, ref uint numOfResults);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_RetrieveResults(string id, uint numOfImages, [Out] SrVADResult[] results);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_InspectVideo(string modelName, string videoPath, ref uint numOfResults);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_RetrieveRevisedResults(string id, string clipPath, uint numOfResults, [Out] SrVADResult[] results);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_ClearResults(string id);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_Reset(string id);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_OverwriteParameters(string id);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_DeleteModel(string id);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_Terminate();

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVAD_SaveMaskContoursInJson(string id, string path);
        #endregion

        #region ImageApi

        [DllImport(SAIGE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int Saige_InitImage(IntPtr image);

        [DllImport(SAIGE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int Saige_CreateImageFromBlob(
            int width, int height, int channels, SaigeDepth depth, ImageDataFormat format, byte[] data, ref SrVADImage image);

        [DllImport(SAIGE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int Saige_ReadImage(string path, IntPtr image);

        [DllImport(SAIGE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int Saige_WriteImage(string path, ref SrVADImage image);

        [DllImport(SAIGE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int Saige_DeleteImage(ref SrVADImage image);

        [DllImport(SAIGE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int Saige_InitBuffer(ref ModelBuffer buffer);

        [DllImport(SAIGE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int Saige_DeleteBuffer(ref ModelBuffer buffer);
        #endregion

        #region TrainApi

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVADTrain_CombineFiles(string modelName, string vadFilePath, string vclsFilePath, string pathToSave);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVADTrain_InitModel(string id, string modelName, SubModelType trainType);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVADTrain_SetParameter(string id, string description, string date, string gpuName, [In] ref VadTrainParams param);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVADTrain_AddClass(string id, string className, int color, int isOk);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVADTrain_AddData(string id, string dataPath);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVADTrain_AddLabel(string id, uint startFrame, uint endFrame, uint classIndex);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVADTrain_SetDevice(string id, DeviceType deviceType, int deviceIndex);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVADTrain_TrainOneIter(string id, ref double loss);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVADTrain_SaveModel(string id, string pathToSave);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVADTrain_DeleteModel(string id);

        [DllImport(VAD_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int SaigeVADTrain_SetCycleInfo(string id, uint cycleIdx, string clipPath, uint startFrame, uint endFrame);
        #endregion

        #region LicenseApi

        [DllImport(SAIGE_LICENSE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern IntPtr Saige_CreateLicenseHandle(
            int numOfLicenses, [In] LicenseType[] licenseType, int checkPeriodInMs, int majorVersion, int minorVersion, int releaseYear, int releaseMonth);

        [DllImport(SAIGE_LICENSE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern int Saige_CheckLicense(
            IntPtr handle, LicenseType licenseType, bool isMultiGpuTrain, bool isMultiGpuTest, bool isUrgent, ref int maxNumOfProcess);

        [DllImport(SAIGE_LICENSE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
        public static extern void Saige_DestroyLicenseHandle(IntPtr handle);
        #endregion
    }

    [SuppressMessage("Performance", "CA1815:값 형식에서 Equals 또는 같음 연산자를 재정의하세요.",
        Justification = "핫픽스를 위해 무시합니다.")]
    public struct ModelBuffer
    {
        public int Length { get; }

        public BufferType Type { get; }

        public IntPtr Data { get; }
    }

    [SuppressMessage("Performance", "CA1815:값 형식에서 Equals 또는 같음 연산자를 재정의하세요.",
        Justification = "핫픽스를 위해 무시합니다.")]
    public struct SrVADImage
    {
        public int Width { get; }

        public int Height { get; }

        public int Channels { get; }

        public IntPtr Data { get; }

        public SaigeDepth Depth { get; }

        public ImageDataFormat Format { get; }
    }

    [SuppressMessage("Performance", "CA1815:값 형식에서 Equals 또는 같음 연산자를 재정의하세요.",
        Justification = "핫픽스를 위해 무시합니다.")]
    public struct SrVADResult
    {
        public long TimeStamp { get; }

        public double NormalizedScore { get; }

        public double Threshold { get; }

        public double BaseScore { get; }

        public double AdaptiveScore { get; }

        public double BaseThreshold { get; }

        public double SlowThreshold { get; }

        public double SlowMean { get; }

        public double SlowVar { get; }

        public double FastThreshold { get; }

        public double FastMean { get; }

        public double FastVar { get; }

        public int ClusterIndex { get; }

        public int AdaptationStep { get; }

        public int FreezeThresholdCount { get; }

        public double HardExampleMiningScore { get; }

        public int IsNormal { get; }

        public int IsMotion { get; }

        public int IsOverwrittenByVcls { get; }

        public int VclsClassIndexOrigin { get; }

        public int VclsClassIndex { get; }

        public int PhaseIndex { get; }

        public int IsInspected { get; }

        public int Roi_X { get; }

        public int Roi_Y { get; }

        public int Roi_W { get; }

        public int Roi_H { get; }

        public ResponseCode ErrorCode { get; }
    }

    public struct VadTrainParams
    {
        public uint Fps;

        public uint BatchSize;

        public uint NumOfWorkers;

        public uint IsOnMemory;

        public int Roi_X;

        public int Roi_Y;

        public int Roi_W;

        public int Roi_H;

        public int NumOfIters;

        public uint NumOfCycles;

        public int MaskType;

        public VadTrainParams(uint fps, uint batchSize, uint numOfWorkers, uint isOnMemory,
            Rectangle roi, int numOfIters, uint numOfCycles, int maskType)
        {
            this.Fps = fps;
            this.BatchSize = batchSize;
            this.NumOfWorkers = numOfWorkers;
            this.IsOnMemory = isOnMemory;
            this.Roi_X = roi.X;
            this.Roi_Y = roi.Y;
            this.Roi_W = roi.Width;
            this.Roi_H = roi.Height;
            this.NumOfIters = numOfIters;
            this.NumOfCycles = numOfCycles;
            this.MaskType = maskType;
        }
    }

    [SuppressMessage("Naming", "CA1720:식별자가 형식 이름을 포함합니다.", Justification = "엔진의 enum을 맵핑합니다.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented",
        Justification = "Summary가 필요없습니다.")]
    public enum BufferType
    {
        UINT = 0,
        INT = 1,
        FLOAT = 2,
        DOUBLE = 3,
        CHAR = 4,
        MODEL_TYPE = 10,
        SUBMODEL_TYPE = 11,
        VAD_RESULT = 12,
        CLS_RESULT = 13,
        VMD_RESULT = 14,
    }

    public enum SaigeDepth
    {
        /// <summary>
        /// 8 bits images
        /// </summary>
        SAIGE_8U = 0,

        /// <summary>
        /// 16 bits images
        /// </summary>
        SAIGE_16U = 1,
    }

    public enum ImageDataFormat
    {
        /// <summary>
        /// index of h-th height, w-th width, c-th channel = h * W * C + w * C + c
        /// </summary>
        HWC = 0,
    }

    public enum DeviceType
    {
        /// <summary>
        /// cpu
        /// </summary>
        CPU = 0,

        /// <summary>
        /// gpu
        /// </summary>
        GPU = 1,
    }

    public enum SubModelType
    {
        UNKNOWN_SUBMODEL = 0,
        CLS = 1,
        DET = 2,
        SEG = 3,
        OCR_BOX = 4,
        OCR_TEXT = 5,
        SUP_RES = 6,
        VIDEO_AD = 7,
        VIDEO_CLS = 8,
        VIDEO_MD = 9,
        OCR_TEXT_INFO = 10,
    }
}
