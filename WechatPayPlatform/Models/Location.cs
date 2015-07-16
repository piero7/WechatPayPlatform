using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatPayPlatform.Models
{
    class Location
    {
        public int LocationId { get; set; }

        public string Address { get; set; }

        public double? X { get; set; }

        public double? Y { get; set; }

    }
}
