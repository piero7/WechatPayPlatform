using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MachineService
{
    class Program
    {
        private static Socket serverSocket;
        public static int myPort = 7777;
        private static byte[] result = new byte[1024];

        static void Main(string[] args)
        {

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myPort));  //绑定IP地址：端口  
            serverSocket.Listen(1);    //设定最多10个排队连接请求  
            Console.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());
            //通过Clientsoket发送数据  
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
            Console.ReadLine();
        }

        private static void ListenClientConnect()
        {
            while (true)
            {
                Socket clientHub = serverSocket.Accept();
                clientHub.Send(Encoding.ASCII.GetBytes("Hello , I`m Ser! "));
                Thread recThread = new Thread(ReceiveMessage);
                recThread.Start(clientHub);
            }


        }

        private static void ReceiveMessage(object obj)
        {
            Socket client = (Socket)obj;
            while (true)
            {
                int retNum = client.Receive(result);
                if (retNum != 0)
                    Console.WriteLine("\t{0}", Encoding.ASCII.GetString(result, 0, retNum));
            }
        }
    }
}
