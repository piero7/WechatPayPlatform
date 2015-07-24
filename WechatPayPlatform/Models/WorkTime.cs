using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatPayPlatform.Models
{
    class WorkTime
    {
        public int WorkTimeId { get; set; }

        public int? StationId { get; set; }

        [ForeignKey("StationId")]
        public virtual Station Station { get; set; }

        public DateTime? LastEditTime { get; set; }

        public string Content { get; set; }

        public string Remarkes { get; set; }

    }
}
