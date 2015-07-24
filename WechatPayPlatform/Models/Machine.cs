using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatPayPlatform.Models
{
    public class Machine
    {
        [Key]
        public int MachineId { get; set; }

        public string Name { get; set; }

        public string InnerId { get; set; }

        public string Address { get; set; }

        public string Remarks { get; set; }

        public MachineType Type { get; set; }

        public string IpAddress { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastConnectedTime { get; set; }

        public int? AdminId { get; set; }

        [ForeignKey("AdminId")]
        public virtual MachineAdmin Admin { get; set; }

    }

    public class MachineMessageLog
    {
        [Key]
        public int MachineMessageLogId { get; set; }

        public string Messgae { get; set; }

        public DateTime? CreateDate { get; set; }

        public MessageType MessageType { get; set; }

        public SocketMessageType Type { get; set; }

    }

    public class MachineAdmin
    {
        [Key]
        public int MachineAdminId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }


        public string Remarks { get; set; }

        public string Count { get; set; }
    }


    /// <summary>
    /// 设备类型
    /// </summary>
    public enum MachineType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 洗车机
        /// </summary>
        CarCleaner = 1,
        /// <summary>
        /// 小洗车机（每次脉冲5毛）
        /// </summary>
        SmallCarCleaner = 2,
    }


    /// <summary>
    /// 消息是请求还是回复
    /// </summary>
    public enum MessageType
    {
        Unknown = 0,
        /// <summary>
        /// 请求
        /// </summary>
        Request = 1,
        /// <summary>
        /// 回复
        /// </summary>
        Response = 2
    }

    /// <summary>
    /// 与Socket服务器报文类型
    /// </summary>
    public enum SocketMessageType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 心跳
        /// </summary>
        Heartbeat = 1,
        /// <summary>
        /// 支付
        /// </summary>
        Pay = 2,
        /// <summary>
        /// 错误信息
        /// </summary>
        ErrorInfo = 3,
    }

    /// <summary>
    /// 自助机状态
    /// </summary>
    public enum MachineStatus
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 使用中
        /// </summary>
        Busy = 1,
        /// <summary>
        /// 空闲
        /// </summary>
        Free = 2,
        /// <summary>
        /// 故障
        /// </summary>
        BreakDwn = 3,
        /// <summary>
        /// 维护中
        /// </summary>
        Maintenance = 4,
    }

}
