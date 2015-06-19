using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WechatPayPlatform.Models
{
    public class PayParms
    {
        public string appId = System.Configuration.ConfigurationManager.AppSettings["appid"];

        public string timestamp { get; set; }

        public string nonceStr { get; set; }

        public string signType = "MD5";

        public string package { get; set; }

        public string paySign { get; set; }

        public void SetPackage(string prePayId)
        {
            this.package = "prepay_id=" + prePayId;
        }
    }
}
