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
        public static HashableProcess FromCurrentProcess()
        {
            HashableProcess returnVal = (HashableProcess)Process.GetCurrentProcess();

            return returnVal;
        }

        public override int GetHashCode()
        {
            return (Id, StartTime).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is HashableProcess other)
            {
                return Id == other.Id && StartTime == other.StartTime;
            }

            return false;
        }
    }
}
