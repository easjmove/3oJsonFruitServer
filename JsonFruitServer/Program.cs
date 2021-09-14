using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using JsonFruit;
using System.Text.Json;

namespace JsonFruitServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Fruit server started");
            TcpListener listener = new TcpListener(IPAddress.Any, 10002);

            listener.Start();
           while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Console.WriteLine("Incoming Client");
                Task.Run(() =>
                {
                    HandleClient(socket);
                });
            }
        }

        private static void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamWriter writer = new StreamWriter(ns);
            StreamReader reader = new StreamReader(ns);
            string message = reader.ReadLine();

            Fruit fromJsonFruit = JsonSerializer.Deserialize<Fruit>(message);
            Console.WriteLine("Client sent Fruit\r\nType of Fruit: " +
                fromJsonFruit.typeOfFruit);
            writer.WriteLine("Fruit received");
            writer.Flush();
            socket.Close();
        }
    }
}
