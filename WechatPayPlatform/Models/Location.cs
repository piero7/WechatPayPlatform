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



        /// <summary>
        /// 获取两点之间的距离
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            double rad = 6371; //Earth radius in Km
            double p1X = x1 / 180 * Math.PI;
            double p1Y = y1 / 180 * Math.PI;
            double p2X = x2 / 180 * Math.PI;
            double p2Y = y2 / 180 * Math.PI;
            return Math.Acos(Math.Sin(p1Y) * Math.Sin(p2Y) +
                Math.Cos(p1Y) * Math.Cos(p2Y) * Math.Cos(p2X - p1X)) * rad;
        }

    }
}
