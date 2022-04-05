using System;
using System.Threading;
using System.Threading.Tasks;
using AxKHOpenAPILib;
using System.Reflection;
using System.Windows.Forms;

namespace EdgeTest
{
    #region Sington Pattern Class
    public class SingletonBase<T> where T : class
    {
        #region Field
        private static T instance;
        #endregion

        private static readonly Lazy<T> Lazy = new Lazy<T>(() =>
        {
            var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (!Array.Exists(ctors, (ci) => ci.GetParameters().Length == 0))
            {
                throw new InvalidOperationException("Non-public ctor() was not found.");
            }

            var ctor = Array.Find(ctors, (ci) => ci.GetParameters().Length == 0);

            return ctor.Invoke(new object[] { }) as T;
        }, LazyThreadSafetyMode.ExecutionAndPublication);

        public static T Instance
        {
            get { return instance ?? (instance = Lazy.Value); }
            set { instance = value; }
        }
    }
    #endregion

    #region KiwoomAPI Wrapping Class
    public abstract class KiwoomAPI
    {
        public static AxKHOpenAPI Get => SingletonBase<APIContext>.Instance.KiwoomAPI;
        private sealed class APIContext : Control
        {
            internal AxKHOpenAPI KiwoomAPI;
            private APIContext()
            {
                Console.WriteLine("sub 01");
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                try
                {
                    KiwoomAPI = new AxKHOpenAPI();
                    Console.WriteLine("sub 02");
                    KiwoomAPI.BeginInit();
                    SuspendLayout();
                    KiwoomAPI.Dock = DockStyle.Fill;
                    Controls.Add(KiwoomAPI);
                    KiwoomAPI.EndInit();
                    ResumeLayout(false);
                }
                catch(Exception err)
                {
                    Console.WriteLine("test" + err.ToString());
                }
            }
        }
    }
        #endregion
    

    public class Startup
    {
        static int a = 0;
        [STAThread]
        public async Task<object> Invoke(dynamic input)
        {
            var aa = new Thread(() =>
            {
                try
                {

                    Console.WriteLine("1");
                    KiwoomAPI.Get.OnEventConnect += Get_OnEventConnect;
                    KiwoomAPI.Get.OnReceiveMsg += Get_OnReceiveMsg;
                    KiwoomAPI.Get.OnReceiveRealData += Get_OnReceiveRealData;
                    KiwoomAPI.Get.OnReceiveTrData += Get_OnReceiveTrData;
                    KiwoomAPI.Get.OnReceiveChejanData += Get_OnReceiveChejanData;
                    Console.WriteLine("2");
                    var nRet = KiwoomAPI.Get.CommConnect();
                    Console.WriteLine("3");
                    if (nRet < 0)
                    {
                        Console.WriteLine("키움증권 접속오류");
                    }
                    else
                    {
                        Console.WriteLine("키움증권 접속성공");
                    }

                }
                catch (Exception err)
                {
                    Console.WriteLine(err.ToString());
                }

                var loop = true;
                while(loop)
                {
                    if(a > 100)
                    {
                        loop = false;
                        continue;
                    }
                    a++;
                    Console.WriteLine("a count : " + a);
                    Thread.Sleep(1000);
                }
            });
            aa.ApartmentState = ApartmentState.STA;
            aa.Start();
            
            return "Test";
        }

        private void Get_OnReceiveChejanData(object sender, _DKHOpenAPIEvents_OnReceiveChejanDataEvent e)
        {
            Console.WriteLine("event connected");
        }

        private void Get_OnReceiveTrData(object sender, _DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            Console.WriteLine("event connected");
        }

        private void Get_OnReceiveRealData(object sender, _DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            Console.WriteLine("event connected");
        }

        private void Get_OnReceiveMsg(object sender, _DKHOpenAPIEvents_OnReceiveMsgEvent e)
        {
            Console.WriteLine("event connected");
        }

        private void Get_OnEventConnect(object sender, _DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            Console.WriteLine("event connected");
        }
    }


}