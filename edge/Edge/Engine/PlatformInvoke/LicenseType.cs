using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaigeVAD.Edge.Engine.PlatformInvoke
{
    public enum LicenseType
    {
        // common
        kDefaultKey = 5,

        // old
        kOldUINTrain = 1,
        kOldClsTest = 2,
        kOldDetTest = 3,
        kOldSegTest = 4,
        kOldUINTest = 20000,
        kOldUINTestPNG = 30000,

        // new
        // ui
        kUIStd = 10001,        // ui standard
        kUIPro = 10002,        // ui pro
        kUIProPlus = 10003,    // ui pro + export all image type
        kUIProDev = 10004,     // ui pro internal use
        kUIVADTrain = 10005,   // ui vad train app
        kUIVADClient = 10006,  // ui vad client app

        // functions
        kClsTrain = 10010,
        kClsTest = 10020,
        kDetTrain = 10030,
        kDetTest = 10040,
        kSegTrain = 10050,
        kSegTest = 10060,
        kGenTrain = 10070,
        kGenTest = 10080,
        kMultiGPUTrain = 10090,
        kMultiGPUTest = 10100,
        kOCRTrain = 10110,
        kOCRTest = 10120,
        kVADTrain = 10130,
        kVADTest = 10140,
        kVClsTrain = 10150,
        kVClsTest = 10160,
        kVMDTrain = 10170,
        kVMDTest = 10180,

        // 아래 항목들은 각 모듈별 학습앱에서의 검사와
        // runtime에서의 검사를 다르게 하기 위한 fake 항목으로
        // 만약 hasp key 등록 등에서 아래 값들이 겹칠경우 언제든지 바꿔줘도 됨.
        // 아래 항목들은 hasp key 에 등록되어 있지 않음
        kClsTestInTrainApp = 100010,
        kDetTestInTrainApp = 100020,
        kSegTestInTrainApp = 100030,
        kOCRTestInTrainApp = 100040,

        // 아래 항목은 라이센스 체크를 하지 않기 위함 (virtual key)
        kFreePass = 100000,
    }

    public enum LicenseErrorType
    {
        kSuccess = 0,
        kNoKey = 1,
        kNoDongle = 2,
        kLowVersion = 3,
        kNullInput = 4,
        kExpired = 5
    }
}
