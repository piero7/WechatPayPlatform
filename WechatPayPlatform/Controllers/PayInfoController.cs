using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml;
using WechatPayPlatform.Models;

namespace WechatPayPlatform.Controllers
{
    public class PayInfoController : ApiController
    {
        /// <summary>
        /// 回传支付信息
        /// </summary>
        /// <param name="non"></param>
        /// <param name="issuccess"></param>
        /// <returns></returns>
        [HttpGet]
        public bool FinishPay(string non, bool issuccess)
        {
            bool ret = false;
            var db = new ModelContext();
            try
            {
                var bill = db.RechargeBillSet.Include("User").FirstOrDefault(b => b.NonceStr == non);
                if (bill != null)
                {
                    bill.LastStatus = "Finish Pay ";
                    if (issuccess)
                    {
                        bill.IsSuccess = true;
                        if (bill.User != null)
                        {
                            if (bill.Count == 1)
                            {
                                // bill.User.Balance += 23;
                            }
                            else
                            {
                                bill.User.Balance += ((bill.Count / 100.00) + ((Convert.ToInt32(bill.Count.Value) / 10000) * 10));
                            }
                        }
                    }
                }
                ret = true;
            }
            finally
            {
                db.SaveChanges();
            }
            return ret;

        }



        /// <summary>
        /// 申请PrePayId并封装支付参数
        /// </summary>
        /// <param name="openid">支付者OpenId</param>
        /// <param name="count">金额</param>
        /// <returns></returns>
        [HttpGet]
        public PayParms GetPayParams(string openid, double count)
        {
            string nonceStr = Helper.GetNonceStr();
            var db = new ModelContext();
            ReachargeBill bill = new ReachargeBill
            {
                CreateDate = DateTime.Now,
                LastStatus = "Create",
                IsSuccess = false,
                NonceStr = nonceStr,
                Count = count,
            };
            db.RechargeBillSet.Add(bill);

            PayParms pp = null;//  = "err";
            try
            {
                bill.LastStatus = "Find User";
                var user = db.WechatUserSet.FirstOrDefault(u => u.OpenId == openid);
                if (user == null)
                {
                    bill.Remarks = "Can`t find user " + openid + "!";
                    return pp;
                }
                bill.UserId = user.UserId;

                //封装Prepayid请求包
                bill.LastStatus = "Create Prepayid Request Package";
                var prePayDic = new Dictionary<string, string>();
                var orderid = DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
                prePayDic.Add("appid", System.Configuration.ConfigurationManager.AppSettings["appid"]);
                prePayDic.Add("body", System.Configuration.ConfigurationManager.AppSettings["paybody"]);
                prePayDic.Add("mch_id", System.Configuration.ConfigurationManager.AppSettings["paymchid"]);
                prePayDic.Add("nonce_str", nonceStr);
                prePayDic.Add("notify_url", "http://www.baidu.com/");
                prePayDic.Add("out_trade_no", orderid);
                prePayDic.Add("spbill_create_ip", System.Configuration.ConfigurationManager.AppSettings["payserviceaddress"]);
                prePayDic.Add("total_fee", count.ToString());
                prePayDic.Add("trade_type", "JSAPI");
                prePayDic.Add("openid", openid);

                bill.OrderId = orderid;

                string sign = Helper.GetMD5(prePayDic);
                prePayDic.Add("sign", sign);


                //请求 PrePayId
                bill.LastStatus = "Get Prepayid ";
                string resStr = Helper.GetResponse(Helper.GetReqStr(prePayDic), "https://api.mch.weixin.qq.com/pay/unifiedorder");

                //Debug
                // db.DebugInfoSet.Add(new DebugInfo(resStr));


                //解析返回数据，获得Prepayid
                int startIndex = resStr.IndexOf("<prepay_id><![CDATA[") + "<prepay_id><![CDATA[".Length;
                int endIndex = resStr.IndexOf("]]></prepay_id>");
                string prepayId = resStr.Substring(startIndex, endIndex - startIndex);

                //封装支付参数包
                bill.LastStatus = "Create Pay Params Package";
                pp = new PayParms();
                pp.timestamp = Helper.getTimestamp();
                pp.nonceStr = nonceStr;
                pp.SetPackage(prepayId);

                Helper.GetMD5(ref pp);

                bill.LastStatus = "Waitting For Finishing Paying";

            }
            catch (Exception ex)
            {
                bill.Remarks = ex.Message;
            }
            finally
            {
                db.SaveChanges();
            }
            return pp;

        }



        /// <summary>
        /// 通过网页获取的code换取用户openid
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetOpenidByCode(string code)
        {
            //  var a = new Helper();
            // a.WriteTxt(code);
            string openid = "err";

            string apps = System.Configuration.ConfigurationManager.AppSettings["appsecrect"];
            string appid = System.Configuration.ConfigurationManager.AppSettings["appid"];
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appid, apps, code);

            string resStr = Helper.GetResponse("", url);

            //a.WriteTxt(resStr);

            //  resStr = string.Format("{{\"res\":{0} }}", resStr);
            var resXml = JsonConvert.DeserializeXNode(resStr, "res");
            var node = resXml.Element("res").Element("openid");
            if (node != null)
            {
                openid = node.Value; ;
            }
            else
            {
                openid = resStr;
            }
            return openid;
        }



        /// <summary>
        /// 获取用户余额
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpGet]
        public double GetBalance(string openid)
        {
            var db = new ModelContext();
            db.DebugInfoSet.Add(new DebugInfo(openid));
            var user = db.WechatUserSet.FirstOrDefault(u => u.OpenId == openid);
            if (user == null)
            {
                return 0;
            }
            else
            {
                return user.Balance.Value;
            }

        }



        /// <summary>
        /// 生成支付订单并通知Socket服务器
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="macid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        public bool ConfirmUse(string openid, string macid, int count)
        {
            var db = new ModelContext();
            var user = db.WechatUserSet.FirstOrDefault(u => u.OpenId == openid);
            if (user == null)
            {
                return false;
            }
            string number = DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
            var bill = new Models.MachineBill
            {
                Count = count,
                CreateDate = DateTime.Now,
                Number = number,
                UserId = user.UserId,
            };
            db.MachineBillSet.Add(bill);
            db.SaveChanges();
            #region debug for user 11
            //if (openid == "11")
            //{
            //    bill.IsSuccess = true;
            //    bill.Remaeks = "Test bill for user 11 .";
            //    db.BillSet.Add(bill);
            //    db.SaveChanges();
            //    return true;
            //}
            #endregion
            // string recvStr = "";
            if (System.Configuration.ConfigurationManager.AppSettings["toservice"] == "false")
            {
                return true;
            }
            else
            {
                SocketController sc = new SocketController();
                sc.SendPayMessage(macid, count, bill.BillId);
            }
            /* bool ret = false;*/
            //验证返回结果
            //             if (recvStr.ToLower() == "success")
            //             {
            //                 ret = true;
            //             }
            //db.BillSet.Add(bill);
            user.Balance -= count;
            db.SaveChanges();
            return true;

        }


        [Obsolete("该方法为测试方法，请误调用。", true)]
        [HttpGet]
        public Models.PayParms GetPayParm()
        {
            var prePayDic = new Dictionary<string, string>();
            string openid = System.Configuration.ConfigurationManager.AppSettings["openId"];
            prePayDic.Add("appid", System.Configuration.ConfigurationManager.AppSettings["appid"]);
            prePayDic.Add("body", "test");
            prePayDic.Add("mch_id", "10037082");
            prePayDic.Add("nonce_str", "1add1a30ac87aa2db72f57a2375d8fec");
            prePayDic.Add("notify_url", "http://www.baidu.com/");
            prePayDic.Add("out_trade_no", "15" + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0'));
            prePayDic.Add("spbill_create_ip", "121.40.79.86");
            prePayDic.Add("total_fee", "1");
            prePayDic.Add("trade_type", "JSAPI");
            prePayDic.Add("openid", openid);

            //        <appid>wx551c10a5e99a84e5</appid>
            //<body>test</body>
            //<mch_id>10037082</mch_id>
            //<nonce_str>1add1a30ac87aa2db72f57a2375d8fec</nonce_str>
            //<notify_url>http://www.baidu.com/</notify_url>
            //<openid>oK7_ajtkA5qGiwrU2MNqa7vs3ttE</openid>
            //<out_trade_no>out_trade_no</out_trade_no>
            //<spbill_create_ip>121.40.79.86</spbill_create_ip>
            //<total_fee>1</total_fee>
            //<trade_type>JSAPI</trade_type>
            //<sign>00492A884A794C6B3892D4406BA6DD8A</sign>
            string sign = Helper.GetMD5(prePayDic);
            prePayDic.Add("sign", sign);
            string resStr = Helper.GetResponse(Helper.GetReqStr(prePayDic), "https://api.mch.weixin.qq.com/pay/unifiedorder");
            int startIndex = resStr.IndexOf("<prepay_id><![CDATA[") + "<prepay_id><![CDATA[".Length;
            int endIndex = resStr.IndexOf("]]></prepay_id>");

            string prepayId = resStr.Substring(startIndex, endIndex - startIndex);

            Models.PayParms pp = new Models.PayParms()
            {
                timestamp = Helper.getTimestamp()
            };
            pp.SetPackage(prepayId);

            Helper.GetMD5(ref pp);

            var db = new Models.ModelContext();
            var user = db.WechatUserSet.FirstOrDefault(u => u.OpenId == openid);
            if (user != null)
            {
                user.Balance += 0.01;
                db.SaveChanges();
            }
            return pp;
        }


        [HttpPost]
        public int GetMid([FromBody]string res)
        {
            var db = new ModelContext();
            var m = db.MachineCodeSet.Include("Machine").FirstOrDefault(item => item.Content == res);
            if (m == null || m.Machine == null)
            {
                return 0;
            }
            return m.MachineId.Value;
        }

        [HttpGet]
        public int GetMachineId(string res)
        {
            var db = new ModelContext();
            var m = db.MachineCodeSet.Include("Machine").FirstOrDefault(item => item.Content == res);
            if (m == null || m.Machine == null)
            {
                return 0;
            }
            return m.MachineId.Value;
        }

        [HttpGet]
        public object HasSales(string openid)
        {
            var db = new ModelContext();
            var user = db.WechatUserSet.FirstOrDefault(item => item.OpenId == openid);
            if (user == null)
            {
                if (string.IsNullOrEmpty(openid) || openid.Contains("errcode"))
                {
                    return null;
                }
                else
                {
                    user = new WechatUser
                    {
                        OpenId = openid,
                        SubscribeTime = DateTime.Now,
                        Balance = 0,
                        subscribe = true,
                        Sex = 0
                    };
                    db.WechatUserSet.Add(user);
                    db.SaveChanges();
                    Helper.GetUserInfo(user);
                }
            }
            if (!db.RechargeBillSet.Any(item => (item.UserId == user.UserId) && (item.IsSuccess ?? false)))
            {
                return new { value = "1", text = "0.01（首单优惠）" };
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public int RefreshAllUserInfo(string pwd)
        {
            if (pwd != "anjismart")
            {
                return 0;
            }
            int ret = 0;
            var db = new ModelContext();
            foreach (var item in db.WechatUserSet)
            {
                if (item.OpenId.Length < 10 || item.OpenId.Contains("{"))
                {
                    continue;
                }
                else
                {
                    Helper.GetUserInfo(item);
                    ret++;
                }
            }
            return ret;
        }

    }

    public partial class Helper
    {
        public static string GetBillStatusName(ComeBillStatus status)
        {
            switch (status)
            {
                case ComeBillStatus.Complain:
                    return "售后处理中";
                case ComeBillStatus.Finish:
                    return "订单完成";
                case ComeBillStatus.ToConfirm:
                    return "等待接单";
                case ComeBillStatus.ToPay:
                    return "已清洗待支付";
                case ComeBillStatus.Working:
                    return "已接单待上门";
                case ComeBillStatus.Cancel:
                    return "取消";
                default:
                    return "其他";
            }
        }
        public static string GetMD5(ref Models.PayParms pp)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("appId", pp.appId);
            dic.Add("timeStamp", pp.timestamp);
            dic.Add("package", pp.package);
            dic.Add("signType", pp.signType);
            dic.Add("nonceStr", pp.nonceStr);
            pp.paySign = GetMD5(dic);

            return pp.paySign;
        }

        public static string GetMD5(Dictionary<string, string> dic)
        {
            var enStr = "";
            ArrayList al = new ArrayList(dic.Keys);
            al.Sort();
            foreach (string item in al)
            {
                enStr += item + "=" + dic[item] + "&";
            }
            enStr += "key=" + System.Configuration.ConfigurationManager.AppSettings["paykey"];
            //  enStr.Replace("timestamp", "timeStamp");
            return GetMD5(enStr);

        }

        public static string GetMD5(string encypStr)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            inputBye = Encoding.UTF8.GetBytes(encypStr);

            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        public static string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public static string GetReqStr(Dictionary<string, string> dic)
        {
            var retStr = "<xml>";
            foreach (var key in dic.Keys)
            {
                retStr += string.Format("<{0}>{1}</{0}>", key, dic[key]);
            }

            retStr += "</xml>";
            return retStr;
        }

        public static string GetResponse(string data, string url)
        {
            HttpWebRequest myHttpWebRequest = null;
            string strReturnCode = string.Empty;
            //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            myHttpWebRequest.ProtocolVersion = HttpVersion.Version10;

            byte[] bs;

            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            bs = Encoding.UTF8.GetBytes(data);

            myHttpWebRequest.ContentLength = bs.Length;

            using (Stream reqStream = myHttpWebRequest.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }


            using (WebResponse myWebResponse = myHttpWebRequest.GetResponse())
            {
                StreamReader readStream = new StreamReader(myWebResponse.GetResponseStream(), Encoding.UTF8);
                strReturnCode = readStream.ReadToEnd();
            }

            return strReturnCode;
        }

        public static string GetNonceStr()
        {
            Random ra = new Random();
            string ret = Helper.getTimestamp().ToString() + ra.Next(9999).ToString();
            return Helper.GetMD5(ret).ToLower();

        }

        internal static void SendWorkMessage(string adminid, string location, CarInfo car, string desc, string time)
        {
            // 新订单通知\n\n位置：小区\n车牌号：沪A12345\n描述：啊啊啊啊啊啊啊啊啊\n服务时间：6/17 12:20~13:20\n<a href=//\"http://www.baiud.com\">点击确认</a>\">

            string msg = "新订单通知\n\n位置:{0}\n车牌号:{1}\n描述:{2}\n服务时间:{3}\n<a href=\\\"{4}\\\">点击确认</a>";
            msg = string.Format(msg, location, car.CarNumber, desc, time, System.Configuration.ConfigurationManager.AppSettings["baseurl"] + "/BillInfo/WorkerBillInfo?adminid=" + adminid.ToString());
            var db = new ModelContext();
            //var admin = db.AdminSet.Find(adminid);

            SendComponyMessage(adminid.ToString(), msg);
        }


        internal static void SendComponyMessage(string account, string msg)
        {
            //   account = "chenzijun|q@51xc.me";
            var send = "{{\"touser\": \"{0}\",\"msgtype\": \"text\",\"agentid\": \"{2}\",\"text\": {{\"content\": \"{1}\"}},\"safe\":\"0\"}}";

            send = string.Format(send, account, msg, System.Configuration.ConfigurationManager.AppSettings["agentid"]);

            var token = GetToken(AccountType.Company);
            var url = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token=" + token;
            GetResponse(send, url);
        }

        public static string GetJsApiSignature(string url, string noncestr, string timestamp)
        {
            var tic = Helper.GetToken(AccountType.Js);
            var cpyString = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", tic, noncestr, timestamp, url);
            var ret = GetSha1Str(cpyString);
            return ret;
        }

        public static string GetJsApiTicket()
        {

            var token = GetToken(AccountType.Service);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi ", token);

            WebClient client = new WebClient();
            string res = client.UploadString(url, "");

            JavaScriptSerializer js = new JavaScriptSerializer();
            var retDic = js.Deserialize<Dictionary<string, string>>(res);
            if (!retDic.Keys.Contains("ticket"))
            {
                return null;
            }

            var ret = retDic["ticket"];

            return ret;
        }
        public static string GetSha1Str(string str)
        {
            byte[] strRes = Encoding.UTF8.GetBytes(str);
            HashAlgorithm iSha = new SHA1CryptoServiceProvider();
            strRes = iSha.ComputeHash(strRes);
            var enText = new StringBuilder();
            foreach (byte iByte in strRes)
            {
                enText.AppendFormat("{0:x2}", iByte);
            }
            return enText.ToString();
        }

        /// <summary>
        /// 支付通知
        /// </summary>
        /// <param name="bill"></param>
        public static void SendFinishCleanMsg(ComeBill bill)
        {
            string msgModle = "{{\"touser\":\"{0}\",\"template_id\":\"Tlkdx1KtnMzGql-cQcO_AakMBxlXX3Ry_DqeovadPjA\",\"url\":\"{1}\",\"topcolor\":\"#FF0000\",\"data\":{{\"first\":{{\"value\":\"{2}\",\"color\":\"#173177\"}},\"ordertape\":{{\"value\":\"{3}\",\"color\":\"#173177\"}},\"ordeID\":{{\"value\":\"{4}\",\"color\":\"#173177\"}},\"remark\":{{\"value\":\"{5}！\",\"color\":\"#173177\"}}}}}}";
            string jurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"] + "/GetMoney/Pay?billnumber=" + bill.innerNumber;
            msgModle = string.Format(msgModle,
                bill.User.OpenId,
                jurl,
                "您的车辆已经清洗完毕，请点击该消息或者进入菜单“钱包”－“最近订单”确认支付，谢谢！",
                bill.CreateDate.Value.ToString("MM月dd日 HH:mm"), bill.innerNumber,
                "感谢您的使用，有任何问题可以直接在微信内输入，或者拨打贵宾专线：4001008262");

            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + Helper.GetToken(AccountType.Service);
            Helper.GetResponse(msgModle, url);
        }

        /// <summary>
        /// 发送确认接单通知
        /// </summary>
        /// <param name="bill"></param>
        public static void SendConfirmMsg(ComeBill bill)
        {
            string msgModle = "{{\"touser\":\"{0}\",\"template_id\":\"5f6csSrXs9Jz1UknK-bA2Mf9g2DB2FdMHR_Ouc-nOyc\",\"url\":\"{1}\",\"topcolor\":\"#FF0000\",\"data\":{{\"first\":{{\"value\":\"{2}\",\"color\":\"#173177\"}},\"keyword1\":{{\"value\":\"{3}\",\"color\":\"#173177\"}},\"keyword2\":{{\"value\":\"{4}\",\"color\":\"#173177\"}},\"keyword3\":{{\"value\":\"{5}\",\"color\":\"#173177\"}},\"remark\":{{\"value\":\"{6}\",\"color\":\"#173177\"}}}}}}";
            string jurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"] + "/billinfo/IndexWithId?billnumber=" + bill.UserId;
            msgModle = string.Format(msgModle,
                bill.User.OpenId,
                jurl,
                "尊敬的客户您好，您的订单已被确认接单。",
                bill.innerNumber,
                bill.Count + " 元",
                bill.CreateDate.Value.ToString("MM月dd日 HH:mm"),
                "我们将会在您预约的时间为您服务。"
                );
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + Helper.GetToken(AccountType.Service);
            Helper.GetResponse(msgModle, url);
        }

        /// <summary>
        /// 发送取消订单通知
        /// </summary>
        /// <param name="bill"></param>
        public static void SendCancelMsg(ComeBill bill)
        {
            string msgModle = "{{\"touser\":\"{0}\",\"template_id\":\"cGQ8AJ4cAX25HowY6xzn_qVpWioAhsWUQ4sfVy06R24\",\"url\":\"{1}\",\"topcolor\":\"#FF0000\",\"data\":{{\"first\":{{\"value\":\"{2}\",\"color\":\"#173177\"}},\"keyword1\":{{\"value\":\"{3}\",\"color\":\"#173177\"}},\"keyword2\":{{\"value\":\"{4}\",\"color\":\"#173177\"}},\"remark\":{{\"value\":\"{5}\",\"color\":\"#173177\"}}}}}}";
            string jurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"] + "/billinfo/IndexWithId?billnumber=" + bill.UserId;
            msgModle = string.Format(msgModle,
                bill.User.OpenId,
                jurl,
                "尊敬的客户您好，您的订单已取消。",
                "上门洗车",
                bill.CreateDate.Value.ToString("MM月dd日 HH:mm"),
                "很遗憾没能为您服务，欢迎下次预约，若有疑问请直接在公众号中回复或拨打客服热线4001008262。"
                );
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + Helper.GetToken(AccountType.Service);
            Helper.GetResponse(msgModle, url);
        }

    }
}
