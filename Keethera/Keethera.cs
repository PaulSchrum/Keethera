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
        public static HashableProcess consoleProcess { get; private set; }
        private static string hiddenConsoleExePath { get; set; }

        public static Guid namedPipeGuid { get; private set; }
        public static string hiddenConsoleName {get {return "KeetheraHiddenConsole";} }

        /// <summary>
        /// Starts an IPC connection. If the broker process has not been started, this
        /// function starts it. If it has been started, it connects to it.
        /// </summary>
        /// <param name="pathToHiddenConsole">Path (only) where to find KeetheraHiddenConsole.exe</param>
        /// <param name="messageReceiver">Method in your app that will receive messages.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static KeetheraInstance Start(string pathToHiddenConsole, 
            Action<KeetheraInstance, string> messageReceiver)
        {
            string openPath = ComposeFileName(pathToHiddenConsole, hiddenConsoleExePath); 
            namedPipeGuid = Guid.NewGuid();
            var currentProcess = HashableProcess.FromCurrentProcess();
            var consoleProcess = GetHiddenConsoleProcess();
            if(consoleProcess is null)
            {  // start the hidden console process.
                consoleProcess = new HashableProcess();
                consoleProcess.StartInfo.FileName = ComposeFileName(pathToHiddenConsole, hiddenConsoleName + ".exe");            
                consoleProcess.StartInfo.Arguments = $"{currentProcess.Id.ToString()} {namedPipeGuid}";
                consoleProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                consoleProcess.StartInfo.CreateNoWindow = true;
                consoleProcess.StartInfo.UseShellExecute = false;
                consoleProcess.StartInfo.RedirectStandardInput = true;
                consoleProcess.StartInfo.RedirectStandardOutput = true;
                consoleProcess.StartInfo.RedirectStandardError = true;
                consoleProcess.StartInfo.LoadUserProfile = false;

                consoleProcess.Start();
            }  // unfinished below this line.
            else
            {  // connect to the hidden console process.
               //                KeetheraProcessID = consoleProcess.Id;
               //                           }}

            }

            throw new NotImplementedException();
            //return new KeetheraInstance();
        }

        private static string ComposeFileName(string pathToHiddenConsole, string exeFileName)
        {
            string directoryPath = pathToHiddenConsole;
            if (!Path.IsPathFullyQualified(pathToHiddenConsole))
            {
                directoryPath = Path.GetFullPath(directoryPath);
            }

            return Path.Combine(directoryPath, exeFileName);
        }

        private static HashableProcess GetHiddenConsoleProcess()
        {
            var procs = Process.GetProcessesByName(hiddenConsoleName);
            if (procs.Length == 0)
                return null;
            return (HashableProcess) procs[0];
        }

        /// <summary>
        /// Shuts down the Keethera hidden console app. Can only be accepted by the same app
        /// that started it.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public static void ShutDown()
        {
            throw new NotImplementedException();
        }

        public static void SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public static void LeaveConversation()
        {
            throw new NotImplementedException();
        }
    }
}
