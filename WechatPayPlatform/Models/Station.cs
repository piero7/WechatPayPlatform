using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace WechatPayPlatform.Models
{
    public class Station
    {
        [Key]
        public int StationId { get; set; }

        public string InnerNumber { get; set; }

        public int? AreaId { get; set; }

        [ForeignKey("AreaId")]
        public virtual Area Area { get; set; }

        public string Name { get; set; }

        public int? AdministratorId { get; set; }

        public virtual Administrator Administrator { get; set; }

        public string Remarks { get; set; }
    }

    public class Area
    {
        [Key]
        public int AreaId { get; set; }

        public string Name { get; set; }

        public  string Discribe { get; set; }

        public string Remarks { get; set; }
    }

    public class Administrator
    {
        [Key]
        public int AdministratorId { get; set; }

        public int? AreaId { get; set; }

        [ForeignKey("AreaId")]
        public virtual Area Area { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string IdNumber { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User UserInfo { get; set; }

        public string Remarks { get; set; }
    }
}