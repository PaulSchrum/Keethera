using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keethera
{

    public class KeetheraInstance
    {
        public Process Pid { get; private set; }
        public Process BrokerPid { get; private set; }

        public KeetheraInstance(Action<string> IncomingMessageHandler)
        {
            Pid = HashableProcess.GetCurrentProcess();
            this.IncomingMessageHandler = IncomingMessageHandler;
            // this.BrokerPid = null;
        }

        /// <summary>
        /// Send a message to all other Keethera instances in the same conversation
        /// or conversations with this one.
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public Action<string> IncomingMessageHandler { get; private set; }
    }
}
