using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
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
        public static KeetheraInstance Start(string pathToHiddenConsole, 
            Action<KeetheraInstance, string> messageReceiver)
        {
            bool startHidden = false;
            string openPath = ComposeFileName(pathToHiddenConsole,
                hiddenConsoleName); 
            namedPipeGuid = Guid.NewGuid();
            var currentProcess = HashableProcess.FromCurrentProcess();
            var consoleProcess = GetHiddenConsoleProcess();
            if(consoleProcess is null)
            {  // start the hidden console process.
                consoleProcess = new HashableProcess();
                consoleProcess.StartInfo.FileName = openPath;
                consoleProcess.StartInfo.Arguments = $"{currentProcess.Id.ToString()} {namedPipeGuid}";
                consoleProcess.StartInfo.UseShellExecute = false;
                consoleProcess.StartInfo.RedirectStandardInput = true;
                consoleProcess.StartInfo.RedirectStandardOutput = true;
                consoleProcess.StartInfo.RedirectStandardError = true;
                consoleProcess.StartInfo.LoadUserProfile = false;
                if (startHidden)
                {
                    consoleProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    consoleProcess.StartInfo.CreateNoWindow = true;
                }
                else
                {
                    // Start the visible console process.
                    consoleProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal; // Changed from Hidden to Normal
                    consoleProcess.StartInfo.CreateNoWindow = false; // Changed from true to false

                }

                consoleProcess.Start();
            }
            else
            {
                NamedPipeClientStream registrationClient = new NamedPipeClientStream(".", "RegistrationPipe", PipeDirection.InOut, PipeOptions.Asynchronous);
                registrationClient.Connect();

                string registrationMessage = $"REGISTER|{currentProcess.Id}|{namedPipeGuid}";
                byte[] messageBytes = Encoding.UTF8.GetBytes(registrationMessage);
                registrationClient.Write(messageBytes, 0, messageBytes.Length);

                // Close the registrationClient after sending the message, as it is no longer needed
                registrationClient.Close();
            }
            throw new NotImplementedException();  ////  ????
            //return new KeetheraInstance();
        }

        private static string ComposeFileName(string pathToHiddenConsole, string exeFileName)
        {
            string directoryPath = pathToHiddenConsole;
            if (!Path.IsPathFullyQualified(pathToHiddenConsole))
            {
                directoryPath = Path.GetFullPath(directoryPath);
            }

            return Path.Combine(directoryPath, exeFileName) + ".exe";
        }

        private static HashableProcess GetHiddenConsoleProcess()
        {
            var procs = Process.GetProcessesByName(hiddenConsoleName);
            if (procs.Length == 0)
                return null;
            return (HashableProcess) procs[0];
        }

        public static string GetPipeName(int procID)
        {
            return $"KeetheraPipe_{procID}";
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
