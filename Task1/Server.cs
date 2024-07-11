using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Task1
{
    internal static class Server
    {
        private static bool _isWorking = true;

        public static void UdpReciever()
        {
            IPEndPoint receiverEndPoint = new IPEndPoint(IPAddress.Any, 0);
            UdpClient udpClient = new UdpClient(12345);
            Console.WriteLine("Receiver is waiting for messages...");
            

            var threadGetKey = new Thread(() =>
            {
                while (true)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                    {
                        _isWorking = false;
                        break;
                    }
                }
                // Отправка сообщения о завершении работы
                IPEndPoint senderEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
                UdpClient udpClient = new UdpClient();
                Message escapeMessage = new Message("Udp Server", "Getting Esc...!\nServer shutdown!");
                string jsonMsg = escapeMessage.GetJson();
                byte[] respondBytes = Encoding.UTF8.GetBytes(jsonMsg);
                udpClient.Send(respondBytes, senderEndPoint);
            });
            threadGetKey.Start();

            while (_isWorking)
            {
                try
                {
                    byte[] bytes = udpClient.Receive(ref receiverEndPoint);
                    string message = Encoding.UTF8.GetString(bytes);

                    var thread = new Thread(() =>
                    {
                        Message? newMessage = Message.GetMessage(message);

                        if (newMessage != null)
                        {
                            if (!_isWorking)
                                Console.WriteLine("x" + newMessage);
                            else
                                Console.WriteLine(newMessage);

                            // Отправка подтверждения получения сообщения
                            Message acceptMessage = new Message("Udp Server", "Message accepted!");
                            string jsonMsg = acceptMessage.GetJson();
                            byte[] respondBytes = Encoding.UTF8.GetBytes(jsonMsg);
                            udpClient.Send(respondBytes, receiverEndPoint);
                        }   
                        else
                            Console.WriteLine("Somthing went wrong with message!");
                    }); 
                    thread.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
