using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 8080;
            TcpListener server = new TcpListener(ipAddress, port);

            server.Start();
            Console.WriteLine("Сервер стартанул на порте: " + port);

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Клиент подключился");

                NetworkStream stream = client.GetStream();
                byte[] request = new byte[4096];
                int bytesRead = stream.Read(request, 0, 4096);
                string requestString = Encoding.UTF8.GetString(request, 0, bytesRead);

                Console.WriteLine("Получен запрос: " + requestString);
                string responseString;
                string responseBody;
                if (requestString.Contains("/start/"))
                {
                    responseBody = "<html><body><h1>Second Page</h1></body></html>";
                }
                else
                {
                    responseBody = "<html><head><style>.center {display: flex;align-items: center;justify-content: center;height: 100vh;}</style></head><body><div class='center'><h1>Begin Page</h1></div></body></html>";
                }
                responseString = "HTTP/1.1 200 OK\nContent-Type: text/html\nContent-Length: " + responseBody.Length + "\n\n" + responseBody;
                byte[] response = Encoding.UTF8.GetBytes(responseString);
                stream.Write(response, 0, response.Length);

                stream.Close();
                client.Close();
            }
        }
    }
}
