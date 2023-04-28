using Keethera;
using KT = Keethera.Keethera;
using System.IO.Pipes;
using System.Text;

namespace KeetheraHiddenConsole
{
    internal class KeetheraHiddenConsole
    {
        public static Dictionary<HashableProcess, PipeAndProcessInfo> participants
        { get; private set; } = null;

        private static NamedPipeServerStream registrationServer { get; set; } = default;

        public const string PublicRequestPipeName = "KeetheraRegistrationPipe";

        static void Main(string[] args)
        {
            int invokingProcessId = Convert.ToInt32(args[1]);
            HashableProcess invokingProcess = (HashableProcess) HashableProcess.GetProcessById(invokingProcessId);
            Console.WriteLine($"Started by Process: {invokingProcess}.");

            registrationServer = new NamedPipeServerStream(PublicRequestPipeName, 
                PipeDirection.InOut, 
                NamedPipeServerStream.MaxAllowedServerInstances, 
                PipeTransmissionMode.Byte, 
                PipeOptions.Asynchronous);

            registrationServer.BeginWaitForConnection(OnClientConnected, registrationServer);

            participants = new Dictionary<HashableProcess, PipeAndProcessInfo>();
            var invokerInfo = new PipeAndProcessInfo(invokingProcess);
            participants.Add(invokingProcess, invokerInfo);
            setUpNamedPipe(invokerInfo);
        }

        private static void setUpNamedPipe(PipeAndProcessInfo invokerInfo)
        {
            throw new NotImplementedException();
        }

        private static void OnClientConnected(IAsyncResult ar)
        {
            NamedPipeServerStream serverStream = (NamedPipeServerStream)ar.AsyncState;
            serverStream.EndWaitForConnection(ar);

            byte[] buffer = new byte[4096];
            int bytesRead = serverStream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // Parse the registration message
            string[] messageParts = message.Split('|');
            if (messageParts[0] == "REGISTER")
            {
                int clientProcessId = int.Parse(messageParts[1]);
                string clientPipeName = messageParts[2];

                // Store the client process ID and pipe name, e.g., in a Dictionary

                // Create a new named pipe server for communication with the client
                NamedPipeServerStream clientServerStream = new NamedPipeServerStream(clientPipeName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                clientServerStream.BeginWaitForConnection(OnClientConnected, clientServerStream);
            }

            // Dispose the serverStream as it is no longer needed for this client
            serverStream.Dispose();

            // Continue waiting for new clients to connect
            registrationServer.BeginWaitForConnection(OnClientConnected, registrationServer);
        }
    }

    public record PipeAndProcessInfo
    {
        public PipeAndProcessInfo(HashableProcess proc)
        {
            process = proc;
            pipeName = $"KeetheraPipe_{proc.Process.Id}";
            pipeName = KT.GetPipeName(proc.Process.Id);
            servingServerStream = new NamedPipeServerStream(
                pipeName, 
                PipeDirection.InOut, 
                NamedPipeServerStream.MaxAllowedServerInstances, 
                PipeTransmissionMode.Byte, 
                PipeOptions.Asynchronous);
        }

        public HashableProcess process { get; init; }
        public string pipeName { get; init; }
        public NamedPipeServerStream servingServerStream { get; init; }
    }

}