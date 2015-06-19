using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatPayPlatform.Models
{
    class DebugInfo
    {
        [Key]
        public int InfoId { get; set; }

        public string Info { get; set; }

        public DateTime Date { get; set; }

        public DebugInfo(string info)
        {
            this.Date = DateTime.Now;
            this.Info = info;
        }
    }
}
