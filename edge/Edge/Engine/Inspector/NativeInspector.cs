using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaigeVAD.Edge.Engine.Inspector
{
    using System;
    using SaigeVAD.Edge.Engine.PlatformInvoke;

    public static class NativeVadInspector
    {
        public static ResponseCode InitImage(IntPtr image)
        {
            int response = NativeMethods.Saige_InitImage(image);

            return (ResponseCode)response;
        }

        public static ResponseCode CreateImageFromBlob(
            int width, int height, int channels, SaigeDepth depth, ImageDataFormat format, byte[] data, ref SrVADImage image)
        {
            int response = NativeMethods.Saige_CreateImageFromBlob(width, height, channels, depth, format, data, ref image);

            return (ResponseCode)response;
        }

        public static ResponseCode ReadImage(string path, IntPtr image)
        {
            int response = NativeMethods.Saige_ReadImage(path, image);

            return (ResponseCode)response;
        }

        public static ResponseCode WriteImage(string path, ref SrVADImage image)
        {
            int response = NativeMethods.Saige_WriteImage(path, ref image);

            return (ResponseCode)response;
        }

        public static ResponseCode DeleteImage(ref SrVADImage image)
        {
            int response = NativeMethods.Saige_DeleteImage(ref image);

            return (ResponseCode)response;
        }

        public static ResponseCode InitBuffer(ref ModelBuffer buffer)
        {
            int response = NativeMethods.Saige_InitBuffer(ref buffer);

            return (ResponseCode)response;
        }

        public static ResponseCode DeleteBuffer(ref ModelBuffer buffer)
        {
            int response = NativeMethods.Saige_DeleteBuffer(ref buffer);

            return (ResponseCode)response;
        }

        public static ResponseCode InitModel(string path, string id)
        {
            int response = NativeMethods.SaigeVAD_InitModel(path, id);

            return (ResponseCode)response;
        }

        public static ResponseCode GetNumOfClasses(string id, ref uint numOfClasses)
        {
            int response = NativeMethods.SaigeVAD_GetNumClasses(id, ref numOfClasses);

            return (ResponseCode)response;
        }

        public static ResponseCode GetSingleClassInfo(string id, int index, ref ModelBuffer param, ref int color)
        {
            int response = NativeMethods.SaigeVAD_GetSingleClassInfo(id, index, ref param, ref color);

            return (ResponseCode)response;
        }

        public static ResponseCode GetModelParameter(string id, string paramName, ref ModelBuffer param)
        {
            int response = NativeMethods.SaigeVAD_GetModelParameter(id, paramName, ref param);

            return (ResponseCode)response;
        }

        public static ResponseCode GetModelParameterWithIndex(
            string id, string paramName, int index, IntPtr param)
        {
            int response = NativeMethods.SaigeVAD_GetModelParameterWithIndex(id, paramName, index, param);

            return (ResponseCode)response;
        }

        public static ResponseCode GetSubModelParameter(
            string id, string subModelName, string paramName, IntPtr param)
        {
            int response = NativeMethods.SaigeVAD_GetSubModelParameter(id, subModelName, paramName, param);

            return (ResponseCode)response;
        }

        public static ResponseCode GetSubModelParameterWithIndex(
            string id, string subModelName, string paramName, int index, IntPtr param)
        {
            int response = NativeMethods.SaigeVAD_GetSubModelParameterWithIndex(
                                                id, subModelName, paramName, index, param);

            return (ResponseCode)response;
        }

        public static ResponseCode SetModelParameter(
            string id, string paramName, string value)
        {
            int response = NativeMethods.SaigeVAD_SetModelParameter(id, paramName, value);

            return (ResponseCode)response;
        }

        public static ResponseCode SetModelParameterWithIndex(
            string id, string paramName, int index, string value)
        {
            int response = NativeMethods.SaigeVAD_SetModelParameterWithIndex(id, paramName, index, value);

            return (ResponseCode)response;
        }

        public static ResponseCode SetSubModelParameter(
            string id, string subModelName, string paramName, string value)
        {
            int response = NativeMethods.SaigeVAD_SetSubModelParameter(id, subModelName, paramName, value);

            return (ResponseCode)response;
        }

        public static ResponseCode SetSubModelParameterWithIndex(
            string id, string subModelName, string paramName, int index, string value)
        {
            int response = NativeMethods.SaigeVAD_SetSubModelParameterWithIndex(
                                                    id, subModelName, paramName, index, value);

            return (ResponseCode)response;
        }

        public static ResponseCode SetDevice(string id, DeviceType deviceType, int deviceIndex)
        {
            int response = NativeMethods.SaigeVAD_SetDevice(id, deviceType, deviceIndex);

            return (ResponseCode)response;
        }

        public static ResponseCode SaveTorchFile(string id, string parentPath)
        {
            int response = NativeMethods.SaigeVAD_SaveTorchFile(id, parentPath);

            return (ResponseCode)response;
        }

        public static ResponseCode PushImage(string id, ref SrVADImage image, long stamp, ref uint numOfResults)
        {
            int response = NativeMethods.SaigeVAD_PushImage(id, ref image, stamp, ref numOfResults);

            return (ResponseCode)response;
        }

        public static ResponseCode RetrieveResults(string id, uint numOfImages, SrVADResult[] results)
        {
            int response = NativeMethods.SaigeVAD_RetrieveResults(id, numOfImages, results);

            return (ResponseCode)response;
        }

        public static ResponseCode InspectVideo(string id, string videoPath, ref uint numOfResults)
        {
            int response = NativeMethods.SaigeVAD_InspectVideo(id, videoPath, ref numOfResults);

            return (ResponseCode)response;
        }

        public static ResponseCode RetrieveRevisedResults(string id, string clipPath, uint numOfResults, SrVADResult[] results)
        {
            int response = NativeMethods.SaigeVAD_RetrieveRevisedResults(id, clipPath, numOfResults, results);

            return (ResponseCode)response;
        }

        public static ResponseCode ClearResults(string id)
        {
            int response = NativeMethods.SaigeVAD_ClearResults(id);

            return (ResponseCode)response;
        }

        public static ResponseCode DeleteModel(string id)
        {
            int response = NativeMethods.SaigeVAD_DeleteModel(id);

            return (ResponseCode)response;
        }

        public static ResponseCode OverwriteParameters(string id)
        {
            int response = NativeMethods.SaigeVAD_OverwriteParameters(id);

            return (ResponseCode)response;
        }

        public static ResponseCode Reset(string id)
        {
            int response = NativeMethods.SaigeVAD_Reset(id);

            return (ResponseCode)response;
        }

        public static ResponseCode Terminate()
        {
            int response = NativeMethods.SaigeVAD_Terminate();

            return (ResponseCode)response;
        }

        public static ResponseCode SaveMaskContoursInJson(string id, string path)
        {
            int response = NativeMethods.SaigeVAD_SaveMaskContoursInJson(id, path);

            return (ResponseCode)response;
        }
    }
}
