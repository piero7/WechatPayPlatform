using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using WechatPayPlatform.Models;

namespace WechatPayPlatform.Controllers
{
    public partial class Helper
    {
        static public WechatUser GetUserInfo(WechatUser user)
        {
            var db = new ModelContext();

            var nUser = db.WechatUserSet.Find(user.UserId);
            //var user = db.UserSet.FirstOrDefault(u => u.OpenId == userOpenId);
            //if (userOpenId == null)
            //{
            //    throw new NotFindCustomerOpenIDException(userOpenId);
            //}

            //if (firm == null)
            //{
            //    throw new NotFindFirmTokenException();
            //}

            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";
            var access = GetToken(AccountType.Service);
            url = string.Format(url, access, user.OpenId);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Timeout = 2000;
            req.Method = "GET";

            var res = (HttpWebResponse)req.GetResponse();
            var s = res.GetResponseStream();
            var sr = new StreamReader(s);
            var resString = sr.ReadToEnd();

            JavaScriptSerializer js = new JavaScriptSerializer();
            var dic = js.Deserialize<Dictionary<string, string>>(resString);
            if (dic.Keys.Contains("errcode"))
            {
                return null;
            }

            user.NickName = dic["nickname"];
            user.Sex = dic["sex"] == "1" ? Sex.男 : dic["sex"] == "2" ? Sex.女 : Sex.未知;
            user.City = dic["city"];
            user.Country = dic["country"];
            user.Province = dic["province"];
            user.Language = dic["language"];
            user.Headimgurl = dic["headimgurl"];

            nUser.NickName = dic["nickname"];
            nUser.Sex = dic["sex"] == "1" ? Sex.男 : dic["sex"] == "2" ? Sex.女 : Sex.未知;
            nUser.City = dic["city"];
            nUser.Country = dic["country"];
            nUser.Province = dic["province"];
            nUser.Language = dic["language"];
            nUser.Headimgurl = dic["headimgurl"];
            db.SaveChanges();

            return user;
        }

        static public string GetToken(AccountType type)
        {
            if (type == AccountType.Service)
            {
                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
                var context = new ModelContext();
                var token = context.AccessTokenSet.First(item => item.Type == AccountType.Service);

                //现有token是否可用
                if (DateTime.Now.Subtract(token.GetTime.Value).TotalSeconds < 7000)
                {
                    return token.Token;
                }


                url = string.Format(url, System.Configuration.ConfigurationManager.AppSettings["appid"], System.Configuration.ConfigurationManager.AppSettings["appsecrect"]);
                #region  获得新的token
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url); ;
                req.Method = "GET";
                req.Timeout = 2000;

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);

                var retString = sr.ReadToEnd();

                JavaScriptSerializer js = new JavaScriptSerializer();
                var retDic = js.Deserialize<Dictionary<string, string>>(retString);

                string newToken = "";
                if (retDic.ContainsKey("access_token"))
                {
                    newToken = retDic["access_token"];
                }
                #endregion
                token.Token = newToken;
                token.GetTime = DateTime.Now;
                context.SaveChanges();

                return newToken;
            }

            if (type == AccountType.Company)
            {
                string url = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}";
                var context = new ModelContext();
                var token = context.AccessTokenSet.First(item => item.Type == AccountType.Company);

                //现有token是否可用
                if (DateTime.Now.Subtract(token.GetTime.Value).TotalSeconds < 6000)
                {
                    return token.Token;
                }


                url = string.Format(url, System.Configuration.ConfigurationManager.AppSettings["eid"], System.Configuration.ConfigurationManager.AppSettings["esecrect"]);
                #region  获得新的token
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url); ;
                req.Method = "GET";
                req.Timeout = 2000;

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);

                var retString = sr.ReadToEnd();

                JavaScriptSerializer js = new JavaScriptSerializer();
                var retDic = js.Deserialize<Dictionary<string, string>>(retString);

                string newToken = "";
                if (retDic.ContainsKey("access_token"))
                {
                    newToken = retDic["access_token"];
                }
                #endregion
                token.Token = newToken;
                token.GetTime = DateTime.Now;
                context.SaveChanges();

                return newToken;
            }
            if (type == AccountType.Js)
            {
                var db = new ModelContext();
                var token = db.AccessTokenSet.FirstOrDefault(item => item.Type == AccountType.Js);
                if (DateTime.Now.Subtract(token.GetTime.Value).TotalSeconds < 6000)
                {
                    return token.Token;
                }


                string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi ", Helper.GetToken(AccountType.Service));

                WebClient client = new WebClient();
                string res = client.UploadString(url, "");

                JavaScriptSerializer js = new JavaScriptSerializer();
                var retDic = js.Deserialize<Dictionary<string, string>>(res);
                if (!retDic.Keys.Contains("ticket"))
                {
                    return null;
                }

                var ret = retDic["ticket"];
                token.GetTime = DateTime.Now;
                token.Token = ret;
                db.SaveChanges();
                return ret;

            }

            return "";
        }
        public bool WriteTxt(string str)
        {
            try
            {
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("/bugLog.txt"), FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入  
                sw.WriteLine(str + "\r\n" + DateTime.Now);
                //清空缓冲区  
                sw.Flush();
                //关闭流  
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static int GetUserCount()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid=";
            var access = GetToken(AccountType.Service);
            url = string.Format(url, access);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Timeout = 2000;
            req.Method = "GET";

            var res = (HttpWebResponse)req.GetResponse();
            var s = res.GetResponseStream();
            var sr = new StreamReader(s);
            var resString = sr.ReadToEnd();

            JavaScriptSerializer js = new JavaScriptSerializer();
            var dic = js.Deserialize<Dictionary<string, object>>(resString);
            if (dic.Keys.Contains("errcode"))
            {
                return 0;
            }
            else if (dic.Keys.Contains("total"))
            {
                return Convert.ToInt32(dic["total"]);
            }
            else
            {
                return 0;
            }
        }
    }
}