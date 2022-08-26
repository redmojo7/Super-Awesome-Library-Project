using System;
using System.Runtime.CompilerServices;

namespace BusinessServer
{
    public class LogClass
    {
        private uint LogNumber = 0;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Log(string logString)
        {
            LogNumber++;
            System.Console.WriteLine(string.Format("[task-{0}][{1}:]{2}",LogNumber, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), logString));
        }
    }
}
