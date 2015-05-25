using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WechatPayPlatform.Controllers
{
    public class SocketController : ApiController
    {
        [HttpGet]
        public bool GetMachine(string machineId)
        {
            throw new NotImplementedException();
            return false;
        }
    }
}
