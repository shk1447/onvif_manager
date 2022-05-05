using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaigeVAD.Edge.Engine.PlatformInvoke
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented",
        Justification = "Summary가 필요없습니다.")]

    public enum ResponseCode
    {
        SUCCESS = 0,
        LICENSE_NOT_FOUND = 101,
        OLD_LICENSE = 102,
        DONGLE_NOT_FOUND = 103,
        MODEL_KEY_NOT_REGISTERED = 104,
        MODEL_KEY_DUPLICATED = 105,
        MODEL_NOT_ALLOC_ON_DEVICE = 106,
        MODEL_ALREADY_BUILT = 107,
        OUT_OF_GPU_MEMORY = 108,
        INV_DEVICE_INDEX = 109,
        NULL_INPUT = 110,
        INV_INPUT = 111,
        FILE_NOT_FOUND = 112,
        INV_FILE_PATH = 113,
        INV_FILE = 114,
        OLD_FILE = 115,
        NEW_FILE = 116,
        INV_IMAGE = 117,
        IMAGE_NOT_INIT = 118,
        TOO_SMALL_IMAGE = 119,
        INV_IMAGE_DEPTH = 120,
        BUFFER_NOT_INIT = 121,
        INV_SUBMODEL_NAME = 122,
        SUBMODEL_NAME_DUPLICATED = 123,
        DIFF_IMAGE_DATA_ORDER = 124,
        TIME_OUT = 125,
        INV_RESULT = 126,
        INV_PARAM_NAME = 127,
        READ_ONLY_PARAM = 128,
        INV_TRAIN_DATA = 129,
        UNKNOWN = 1000,
    }
}
