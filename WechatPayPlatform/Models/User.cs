
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WechatPayPlatform.Models
{
    public class WechatUser
    {
        [Key]
        public int UserId { get; set; }

        public string OpenId { get; set; }

        public string NickName { get; set; }

        public bool subscribe { get; set; }

        [EnumDataType(typeof(Sex))]
        public Sex Sex { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Province { get; set; }

        public string Language { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? SubscribeTime { get; set; }

        public string Headimgurl { get; set; }

        public double? Balance { get; set; }

        public string Remarks { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public int? StationId { get; set; }

        [ForeignKey("StationId")]
        public virtual Station Station { get; set; }
    }


    public class UserInfo
    {
        public int UserInfoId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public int? StationId { get; set; }

        [ForeignKey("StationId")]
        public virtual Station Station { get; set; }

        public string Remarks { get; set; }
    }


    public class CarInfo
    {
        [Key]
        public int CarId { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual WechatUser UserInfo { get; set; }

        public double? LocationX { get; set; }

        public double? LocationY { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNumber { get; set; }

        public int? NeighborhoodId { get; set; }

        [ForeignKey("NeighborhoodId")]
        public virtual Neighborhood Nieghborhood { get; set; }

        public DateTime? LastUseDate { get; set; }

        public DateTime? CreateDate { get; set; }

        public string Describe { get; set; }

        public string LocationDescribe { get; set; }

    }



    /// <summary>
    /// 小区
    /// </summary>
    public class Neighborhood
    {
        public int NeighborhoodId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int? StationId { get; set; }

        [ForeignKey("StationId")]
        public virtual Station Station { get; set; }

        public string Reamarks { get; set; }
    }




    public enum Sex
    {
        男 = 1,
        女 = 2,
        未知 = 0,
    }

    public class AccessToken
    {
        [Key]
        public int AcceccTokenId { get; set; }

        public string Token { get; set; }

        public DateTime? GetTime { get; set; }

        public AccountType Type { get; set; }

    }

    public enum AccountType
    {
        Unknown = 0,
        Service = 1,
        Company = 2,
        Js=3,
    }

}