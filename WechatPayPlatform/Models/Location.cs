using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatPayPlatform.Models
{
    class Location
    {
        public double X { get; set; }

        public double Y { get; set; }

        public Location(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

    }
}
