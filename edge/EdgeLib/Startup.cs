using System;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace EdgeLib
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

    public class Startup
    {
        static int a = 0;
        public async Task<object> Invoke(dynamic input)
        {
            var aa = new Thread(() =>
            {
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
            aa.Start();
            
            return "Test";
        }
    }
}