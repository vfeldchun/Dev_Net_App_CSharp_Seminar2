using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Task1
{
    internal static class Client
    {
        public static void UdpSender(string name)
        {
            IPEndPoint senderEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            UdpClient udpClient = new UdpClient();

            while (true)
            {
                Console.WriteLine("Введите сообщение:");
                string? messageText = Console.ReadLine();

                if (String.IsNullOrEmpty(messageText) || messageText.ToLower() == "exit") break;

                Message newMessage = new Message(name, messageText);
                string jsonMsg = newMessage.GetJson();
                byte[] bytes = Encoding.UTF8.GetBytes(jsonMsg);
                udpClient.Send(bytes, senderEndPoint);

                // Получение подтверждения получения сообщения
                byte[] acceptBytes = udpClient.Receive(ref senderEndPoint);
                string message = Encoding.UTF8.GetString(acceptBytes);
                Message? acceptMessage = Message.GetMessage(message);
                Console.WriteLine(acceptMessage);
            }
        }
    }
}
