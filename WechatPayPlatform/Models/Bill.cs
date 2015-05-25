using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WechatPayPlatform.Models
{
    public class Bill
    {
        [Key]
        public int BillId { get; set; }

        public string Number { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public DateTime? CreateDate { get; set; }

        public double Count { get; set; }

        public bool IsSuccess { get; set; }

        public string Remaeks { get; set; }
    }

    public class ReachargeBill
    {
        [Key]
        public int RechargeBillId { get; set; }

        public string OrderId { get; set; }

        public string LastStatus { get; set; }

        public DateTime CreateDate { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public string Remarks { get; set; }

        public bool? IsSuccess { get; set; }

        public double? Count { get; set; }

        public string NonceStr { get; set; }
    }
}