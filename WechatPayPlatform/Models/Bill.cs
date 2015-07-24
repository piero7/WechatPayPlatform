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
        public virtual WechatUser User { get; set; }

        public DateTime? CreateDate { get; set; }

        public double Count { get; set; }

        public bool IsSuccess { get; set; }


        public string innerNumber { get; set; }

        public string Remarks { get; set; }

        public ComeBillStatus Status { get; set; }

        public DateTime? PayDate { get; set; }

    }



    /// <summary>
    /// 自助洗订单
    /// </summary>
    public class MachineBill : Bill
    {
        public int? MachineId { get; set; }

        public virtual Machine Machine { get; set; }

    }

    public class ShopBill : Bill
    {
        public int? ShopId { get; set; }

        [ForeignKey("ShopId")]
        public virtual Shop Shop { get; set; }
    }

    /// <summary>
    /// 上门洗订单
    /// </summary>
    public class ComeBill : Bill
    {
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string PhoneNumber { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNumber { get; set; }

        public string CarInfo { get; set; }

        public string Address { get; set; }

        public string Describe { get; set; }

        public DateTime? FinishTime { get; set; }

        public int? AdminId { get; set; }

        [ForeignKey("AdminId")]
        public virtual Administrator Admin { get; set; }

        public int? ComeBillTypeId { get; set; }

        [ForeignKey("ComeBillTypeId")]
        public ComeBillType BillType { get; set; }

        public double? LocationX { get; set; }

        public double? LocationY { get; set; }

        public int? NeighborhoodId { get; set; }

        [ForeignKey("NeighborhoodId")]
        public virtual Neighborhood Neighborhood { get; set; }

    }

    public class ComeBillType
    {
        [Key]
        public int ComeBillTypeId { get; set; }

        public string Name { get; set; }

        public double? Price { get; set; }

        public string Describe { get; set; }

        public string ExplainUrl { get; set; }

        public string Remarks { get; set; }


    }


    /// <summary>
    /// 充值订单
    /// </summary>
    public class ReachargeBill
    {
        [Key]
        public int RechargeBillId { get; set; }

        public string OrderId { get; set; }

        public string LastStatus { get; set; }

        public DateTime CreateDate { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual WechatUser User { get; set; }

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
        Shop = 1,
        /// <summary>
        /// 自助洗
        /// </summary>
        Diy = 2,
        /// <summary>
        /// 上门
        /// </summary>
        Come = 3,
    }

    public enum ComeBillStatus
    {
        Unknown = 0,
        ToConfirm = 1,
        Working = 2,
        ToPay = 3,
        Finish = 4,
        Complain = 10,
        Cancel = 9,
    }
}