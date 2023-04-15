using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keethera
{
    public static class Keethera
    {
        public static int KeetheraProcessID { get; private set; }

        public static KeetheraInstance Start(Action<KeetheraInstance, string> messageReceiver)
        {
            Process currentProcess = Process.GetCurrentProcess();
            int pid = Process.GetCurrentProcess().Id;
            DateTime startTime = Process.GetCurrentProcess().StartTime;

            throw new NotImplementedException();
            //return new KeetheraInstance();
        }
    }
}
