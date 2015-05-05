using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Web.Http;
using WechatPayPlatform.Models;

namespace WechatPayPlatform.Controllers
{
    public class PayInfoController : ApiController
    {
        [HttpGet]
        public double GetBalance(string openid)
        {
            var db = new ModelContext();
            var user = db.UserSet.FirstOrDefault(u => u.OpenId == openid);
            if (user == null)
            {
                return 0;
            }
            else
            {
                return user.Balance.Value;
            }

        }

        [HttpGet]
        public bool ConfirmUse(string openid, string macid, int count)
        {
            var db = new ModelContext();
            var user = db.UserSet.FirstOrDefault(u => u.OpenId == openid);
            if (user == null)
            {
                return false;
            }
            string number = DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
            var bill = new Models.Bill
            {
                Count = count,
                CreateDate = DateTime.Now,
                Number = number,
                UserId = user.UserId,
            };
            #region debug for user 11
            if (openid == "11")
            {
                bill.IsSuccess = true;
                bill.Remaeks = "Test bill for user 11 .";
                db.BillSet.Add(bill);
                db.SaveChanges();
                return true;
            }
            #endregion
            int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["serviceport"]);
            string host = System.Configuration.ConfigurationManager.AppSettings["serviceaddress"];
            //创建终结点EndPoint
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);   //把ip和端口转化为IPEndPoint的实例

            //创建Socket并连接到服务器
            Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);   //  创建Socket

            c.Connect(ipe); //连接到服务器

            string noncestr = "";
            string time = Helper.getTimestamp();
            var token = Helper.GetMD5(macid + count.ToString() + noncestr + number + time + System.Configuration.ConfigurationManager.AppSettings["servicetoken"]);
            //向服务器发送信息
            string sendStr = string.Format("macid={0}&count={1}&noncestr{2}&orderid={3}&time={5}&token={4}", macid, count, noncestr, number, token, Helper.getTimestamp());
            byte[] bs = Encoding.ASCII.GetBytes(sendStr);   //把字符串编码为字节

            c.Send(bs, bs.Length, 0); //发送信息

            //接受从服务器返回的信息
            string recvStr = "";
            byte[] recvBytes = new byte[1024];
            int bytes;
            bytes = c.Receive(recvBytes, recvBytes.Length, 0);    //从服务器端接受返回信息
            recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);
            c.Close();

            bool ret = false;
            //验证返回结果
            if (recvStr.ToLower() == "success")
            {
                ret = true;
            }
            db.BillSet.Add(bill);
            db.SaveChanges();
            return ret;

        }
    }
}
