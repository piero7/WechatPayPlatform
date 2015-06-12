using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace WechatPayPlatform.Models
{
    public class Code
    {
        [Key]
        public int CodeId { get; set; }

        public string Url { get; set; }

        public string Content { get; set; }

        public string EventKey { get; set; }
    }

    public class MachineCode : Code
    {
        public int? MachineId { get; set; }

        [ForeignKey("MachineId")]
        public virtual Machine Machine { get; set; }
    }

    public class StationCode : Code
    {
        public int? StationId { get; set; }

        [ForeignKey("StationId")]
        public virtual Station Station { get; set; }
    }

    //  public class Machine
    //{
    //    [Key]
    //    public int MachineId { get; set; }

    //    public string Name { get; set; }

    //    public string InnerId { get; set; }

    //    public string Address { get; set; }

    //    public string Remarks { get; set; }

    //}
}