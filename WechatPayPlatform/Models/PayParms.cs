using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WechatPayPlatform.Models
{
   public class PayParms
    {
        public string appId = System.Configuration.ConfigurationManager.AppSettings["appid"];

        public string timeStamp { get; set; }

        public string nonceStr = "e61463f8efa94090b1f366cccfbbb444";

        public string signType = "MD5";

        public string package { get; set; }

        public string paySign { get; set; }

        public void SetPackage(string prePayId)
        {
            this.package = "prepay_id=" + prePayId;
        }
    }
}
