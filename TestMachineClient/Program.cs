using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TestMachineClient
{
    class Program
    {
        private static byte[] result = new byte[1024];

        static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            //Console.WriteLine("plase input the ip address:");
            //ip = Console.ReadLine();
            int port = 7777;
            //Console.WriteLine("Plase input the port :");
            //port = int.Parse(Console.ReadLine());

            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {

                client.Connect(IPAddress.Parse(ip), port);
                Console.WriteLine("Connect success!");
            }
            catch (Exception e)
            {

                Console.WriteLine("failed!!\r\n " + e.Message);
            }

            //int retNum = client.Receive(result);
           // Console.WriteLine("Get reserve message :{0}", Encoding.ASCII.GetString(result, 0, retNum));
            string sendMessage = "";
            int i = 0;
            while (true)
            {
                sendMessage = Console.ReadLine();
                if (sendMessage == "exit")
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    client.Dispose();
                    break;
                }
                try
                {
                    Thread.Sleep(500);
                    client.Send(Encoding.ASCII.GetBytes(sendMessage));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    client.Dispose();
                    //return;
                }
            }
            //client.Close();

            Console.ReadKey();


        }
    }
}
