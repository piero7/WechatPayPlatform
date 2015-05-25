using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatPayPlatform.Models
{
    class Machine
    {
        [Key]
        public int MachineId { get; set; }

        public string Name { get; set; }

        public string MachineNumber { get; set; }

        public string Address { get; set; }

        public string Remarks { get; set; }

        public MachineType Type { get; set; }

    }

    public enum MachineType
    {
        Unknown = 0,
        CarCleaner = 1,
    }
}
