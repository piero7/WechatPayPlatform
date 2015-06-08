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
        public static int myPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["port"]);
        public static string address = System.Configuration.ConfigurationManager.AppSettings["address"];

        private static List<SocketClient> clientList = new List<SocketClient>();


        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse(address);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myPort));  //绑定IP地址：端口  
            serverSocket.Listen(100);    //设定最多10个排队连接请求  
            Console.WriteLine("启动监听{0}成功\t{1}", serverSocket.LocalEndPoint.ToString(), DateTime.Now.ToString());
            // serverSocket.BeginAccept(new AsyncCallback(OnConnectRequest), serverSocket);

            //通过Clientsoket发送数据  
            Thread myThread = new Thread(new ThreadStart(ListenClientConnect));
            myThread.Start();
            while (true)
            {

                var input = Console.ReadLine();

                if (input == "showlist" || input == "sl")
                {
                    foreach (var item in clientList)
                    {
                        Console.WriteLine("\t{0}\t{1}\t{2}", item.Type.ToString(), item.MachineNumber, item.CreateDate.ToString());
                    }
                }
                Console.WriteLine("\r");
            }
        }

        private static void ListenClientConnect()
        {
            while (true)
            {
                Socket clientHub = serverSocket.Accept();
                var socketClient = new SocketClient(clientHub);
                clientList.Add(socketClient);
                //clientHub.Accept();
                // clientHub.Send(Encoding.ASCII.GetBytes("Hello , I`m Ser! "));
                Thread recThread = new Thread(ReceiveMessage);
                recThread.Start(socketClient);
                //Thread.Sleep(500);
            }
        }



        private static void ReceiveMessage(object obj)
        {
            SocketClient client = (SocketClient)obj;
            while (true)
            {
                try
                {
                    byte[] result = new byte[1024];
                    int retNum = client.Socket.Receive(result);
                    if (client.Type == SocketClientType.Unknown)
                    {
                        if (retNum == 8)
                        {
                            client.Type = SocketClientType.Machine;
                        }
                        else if (retNum == 15)
                        {
                            client.Type = SocketClientType.Service;
                        }
                    }
                    if (client.Type == SocketClientType.Machine)
                    {
                        if (retNum == 8)
                        {
                            var resultStr = Encoding.ASCII.GetString(result).Substring(0, 8);

                            string mNumber = resultStr.Substring(0, 4);

                            //删除冲突链接
                            if (client.MachineNumber != mNumber)
                            {
                                foreach (var item in clientList.Where(c => c.MachineNumber == mNumber))
                                {
                                    item.Socket.Close();
                                    item.Socket.Dispose();
                                }
                                clientList.RemoveAll(item => item.MachineNumber == mNumber);
                                client.MachineNumber = mNumber;
                            }

                            Console.WriteLine("Get machine message: {0}  {1}", resultStr, DateTime.Now.ToString());
                            OptionData(resultStr);
                            client.LastDate = DateTime.Now;

                            client.Socket.Send(Encoding.ASCII.GetBytes(resultStr.ToUpper()));
                        }
                            //ADXC40112345678
                        else if(retNum== 15)
                        {
                            
                        }
                        Console.WriteLine("\r");
                    }
                    else if (client.Type == SocketClientType.Service)
                    {
                        if (retNum == 15)
                        {
                            var resultStr = Encoding.ASCII.GetString(result).Substring(0, 15);

                            Console.WriteLine("Request from service: {0}  {1}", resultStr, DateTime.Now.ToString());
                            var maClient = clientList.FirstOrDefault(item => item.MachineNumber.ToLower() == resultStr.Substring(0, 4).ToLower());
                            if (maClient == null)
                            {
                                Console.WriteLine("Can`t find the machine!");
                                Console.WriteLine("\r");
                                break;
                            }
                            maClient.Socket.Send(Encoding.ASCII.GetBytes(resultStr.ToUpper()));
                        }
                        else if (retNum < 1)
                        {
                            break;
                        }
                        client.Socket.Close(200);
                        client.Socket.Dispose();
                        clientList.Remove(client);
                        Console.WriteLine("\r");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR!!!!\r\t{0}\t{1}\r", ex.Message, DateTime.Now.ToString());
                    break;
                }
            }
        }

        private static bool OptionData(string resultStr)
        {
            WebClient wc = new WebClient();
            var retBytes = wc.DownloadData(string.Format(System.Configuration.ConfigurationManager.AppSettings["serviceaddress"], resultStr));
            var ret = Encoding.ASCII.GetString(retBytes);
            if (ret == "true")
            {
                Console.WriteLine("Notice servie success");
                return true;
            }
            else
            {
                Console.WriteLine(ret);
                return false;
            }
        }
    }

    internal class SocketClient
    {
        public Socket Socket { get; set; }

        public bool IsNeedRequest { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastDate { get; set; }

        public List<byte> Buffer { get; set; }

        public string MachineNumber { get; set; }

        public SocketClientType Type { get; set; }

        public SocketClient(Socket socket)
        {
            this.Socket = socket;
            this.IsNeedRequest = false;
            this.CreateDate = DateTime.Now;
            this.LastDate = DateTime.Now;
            this.Type = SocketClientType.Unknown;
            this.MachineNumber = "";
        }


    }
    enum SocketClientType
    {
        Unknown = 0,
        Service = 1,
        Machine = 2,
    }


}
