
namespace Task1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Server.UdpReciever();
            }
            else
            {
                new Thread(() =>
                {
                    Client.UdpSender($"{args[0]}");
                }).Start();
            }
        }
    }
}
