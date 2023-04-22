using Keethera;


namespace ExampleConsoleApp1
{
    internal class Program
    {
        private static string KeetheraPath = 
            @"..\KeetheraHiddenConsole\bin\Debug\netcoreapp3.1";

        private static KeetheraInstance messageHandling = null;
        private static TimeSpan delayTime = TimeSpan.Zero;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Example Console App 1 started.");
            //messageHandling =
            //    Keethera.Keethera.Start(KeetheraPath, incomingMessageHandler);

            Random random = new Random();
            while(true)
            {
                delayTime = TimeSpan.FromSeconds(2 + 3*random.NextDouble());
                await Task.Delay(delayTime);
                //messageHandling.SendMessage(Guid.NewGuid().ToString());
            }
        }

        private static string GUID()
        {
            return Guid.NewGuid().ToString();
        }

        private static void incomingMessageHandler(KeetheraInstance arg1, string arg2)
        {
            Console.WriteLine($"Received Message: {arg2}");
            throw new NotImplementedException();
        }
    }
}