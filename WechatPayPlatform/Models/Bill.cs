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

        public int? MachineId { get; set; }

        public virtual Machine Machine { get; set; }

        

        public string innderNumber { get; set; }

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

    public enum BillStatus
    {
        Unknown = 0,
        Sended = 1,
        Success = 2,
        Error = 3,
        Create = 4,
    }

    public enum BillType
    {
        Unknown = 0,
        /// <summary>
        /// 到店
        /// </summary>
        Shop =1,
        /// <summary>
        /// 自助洗
        /// </summary>
        Diy =2,
        /// <summary>
        /// 上门
        /// </summary>
        Come=3,
    }
}