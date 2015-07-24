using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WechatPayPlatform.Models
{
    class ModelContext : DbContext
    {
        public ModelContext() : base("ModelContextConnString") { }

        #region User

        public DbSet<WechatUser> WechatUserSet { get; set; }

        public DbSet<UserInfo> UserInfoSet { get; set; }

        public DbSet<CarInfo> CarInfoSet { get; set; }
        #endregion

        #region Bills
        public DbSet<MachineBill> MachineBillSet { get; set; }


        public DbSet<ReachargeBill> RechargeBillSet { get; set; }

        public DbSet<ComeBill> ComeBillSet { get; set; }

        public DbSet<ComeBillType> ComeBillType { get; set; }

        #endregion

        #region Machine
        public DbSet<Machine> MachineSet { get; set; }

        public DbSet<MachineMessageLog> MachineMessageSet { get; set; }

        public DbSet<MachineAdmin> MachineAdminSet { get; set; }

        public DbSet<MachineCode> MachineCodeSet { get; set; }
        #endregion

        #region Statino
        public DbSet<StationCode> StationCodeSet { get; set; }

        public DbSet<Station> StationSet { get; set; }

        public DbSet<Neighborhood> NeighborhoodSet { get; set; }

        public DbSet<Area> AreaSet { get; set; }

        public DbSet<Administrator> AdminSet { get; set; }
        #endregion

        #region Shop
        public DbSet<Shop> ShopSet { get; set; }

        public DbSet<Package> PackageSet { get; set; }

        public DbSet<ShopBill> ShopBillSet { get; set; }
        #endregion

        #region Score
        public DbSet<Score> ScoreSet { get; set; }

        public DbSet<ScoreLog> ScoreLogSet { get; set; }
        #endregion

        public DbSet<AccessToken> AccessTokenSet { get; set; }

        public DbSet<DebugInfo> DebugInfoSet { get; set; }

        public DbSet<WorkTime> WorkTimeSet { get; set; }

    }
}
