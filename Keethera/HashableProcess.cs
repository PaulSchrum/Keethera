using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keethera
{
    public class HashableProcess : Process
    {
        private Process myProcess = null;
        public Process Process { get { return myProcess; } }

        public static HashableProcess FromCurrentProcess()
        {
            HashableProcess returnVal = new HashableProcess();
            returnVal.myProcess = Process.GetCurrentProcess();

            return returnVal;
        }

        public static HashableProcess FromProcessFromID(int processID)
        {
            HashableProcess returnVal = new HashableProcess();
            var process = Process.GetProcessById(processID);
            returnVal.myProcess = process;
            return returnVal;
        }

        public override int GetHashCode()
        {
            return (myProcess.Id, myProcess.StartTime).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is HashableProcess other)
            {
                return (Id == other.Id) && (StartTime == other.StartTime);
            }

            return false;
        }

    }
}
