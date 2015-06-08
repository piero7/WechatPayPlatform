using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatPayPlatform.Models
{
  public  class Machine
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


    /// <summary>
    /// 设备类型
    /// </summary>
    public enum MachineType
    {
        Unknown = 0,
        CarCleaner = 1,
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

    public enum SocketMessageType
    {
        Unknown = 0,
        Heartbeat = 1,
        Pay = 2,
        ErrorInfo = 3,
    }

}
