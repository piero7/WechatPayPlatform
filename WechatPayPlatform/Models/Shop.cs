using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WechatPayPlatform.Models
{
    public class Shop
    {
        [Key]
        public int ShopId { get; set; }

        public string Address { get; set; }

        public double? LocationX { get; set; }

        public double? LocationY { get; set; }

        public string Phone { get; set; }

        public string AdminName { get; set; }

        public string Name { get; set; }

        public string Remarks { get; set; }

        public string ImgPath { get; set; }

        /// <summary>
        /// 最低消费
        /// </summary>
        public double? Minimum { get; set; }

        /// <summary>
        /// 营业时间
        /// </summary>
        public string ShopHours { get; set; }

        public int? ScoreId { get; set; }

        [ForeignKey("ScoreId")]
        public virtual Score Score { get; set; }
    }


    public class Package
    {

        [Key]
        public int PackaheId { get; set; }

        public string Name { get; set; }

        public int? ShopId { get; set; }

        [ForeignKey("ShopId")]
        public virtual Shop Shop { get; set; }

        public double Price { get; set; }

    }
}