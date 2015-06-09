using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Web.Http;



namespace WechatPayPlatform.Controllers
{
    public class SocketController : ApiController
    {
        //public static List<Models.Bill>

        [HttpGet]
        public bool RefreshOrderStatus(string machineNumber, string orderNumber)
        {
            return false;
        }

        /// <summary>
        /// Socket服务器提交设备上线注册及心跳
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        [HttpGet]
        public bool GetMachine(string message)
        {
            var db = new Models.ModelContext();
            db.MachineMessageSet.Add(new Models.MachineMessageLog
            {
                MessageType = Models.MessageType.Request,
                CreateDate = DateTime.Now,
                Type = Models.SocketMessageType.Heartbeat,
                Messgae = message
            });

            //ADXC0001
            if (message.Length != 8)
            {
                return false;
            }

            string machineNumber = message.Substring(0, 4);

            string messageFlg = message.Substring(4);
            var machine = db.MachineSet.FirstOrDefault(item => item.InnerId.ToLower() == machineNumber.ToLower());

            if (machine == null)
            {
                return false;
            }
            machine.IsActive = true;
            machine.LastConnectedTime = DateTime.Now;

            db.SaveChanges();
            return true;

            //throw new NotImplementedException();
            //   return false;
        }


        /// <summary>
        /// 发送支付消息给设备
        /// </summary>
        /// <param name="machineNumber">设备号</param>
        /// <param name="orderNumber">订单号</param>
        /// <param name="count">金额</param>
        //[HttpGet]
        public void SendPayMessage(string machineNumber, int count, int billId)
        {

            //ADXC 001 12345678
            string orderStr = "";
            orderStr += GetHexString(DateTime.Now.Year - 2015);
            orderStr += GetHexString(DateTime.Now.Month);
            orderStr += DateTime.Now.Day.ToString().PadLeft(2, '0');

            var random = new Random();
        getRandom: orderStr += random.Next(9999).ToString().PadLeft(4, '0');
            var db = new Models.ModelContext();
            if (db.BillSet.Any(item => item.innderNumber == orderStr))
            {
                goto getRandom;
            }
            db.BillSet.FirstOrDefault(item => item.BillId == billId).innderNumber = orderStr;
            db.SaveChanges();

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(machineNumber.ToUpper() + GetHexString(count).PadLeft(3, '0') + orderStr);

            Socket so = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            so.Connect(System.Configuration.ConfigurationManager.AppSettings["serviceaddress"], int.Parse(System.Configuration.ConfigurationManager.AppSettings["serviceport"]));
            so.Send(msg);

            // so.Disconnect(false);
            so.Close(300);
            so.Dispose();
        }

        private static string GetHexString(int number)
        {
            if (number > 15 || number < 0)
            {
                return null;
            }
            if (number < 10)
            {
                return number.ToString();
            }
            if (number == 10)
            {
                return "a";
            }
            if (number == 11)
            {
                return "b";
            } if (number == 12)
            {
                return "c";
            } if (number == 13)
            {
                return "d";
            } if (number == 14)
            {
                return "e";
            } if (number == 15)
            {
                return "f";
            }
            else
            {
                return null;
            }
        }

    }
}
