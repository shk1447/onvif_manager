using System;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;

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
        public class ConsoleSpiner
        {
            int counter;
            public ConsoleSpiner()
            {
                counter = 0;
            }
            public void Turn()
            {
                counter++;
                switch (counter % 4)
                {
                    case 0: Console.Write("/"); break;
                    case 1: Console.Write("-"); break;
                    case 2: Console.Write("\\"); break;
                    case 3: Console.Write("|"); break;
                }
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
        }

        static Thread _thread = new Thread(() =>
        {
            ConsoleSpiner spiner = new ConsoleSpiner();
            while (true)
            {
                spiner.Turn();
                Thread.Sleep(100);
            }
        });
        public async Task<object> Invoke(dynamic input) {
            if(input.state)
            {
                _thread.Start();
            }
            else
            {
                _thread.Abort();
            }
            
            return input.state;
        }
    }
}