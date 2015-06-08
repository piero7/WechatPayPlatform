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

        public DbSet<User> UserSet { get; set; }

        public DbSet<Code> CodeSet { get; set; }

        // public DbSet<Machine> MachineSet { get; set; }

        public DbSet<AccessToken> AccessTokenSet { get; set; }

        public DbSet<Bill> BillSet { get; set; }

        public DbSet<ReachargeBill> RechargeBillSet { get; set; }

        public DbSet<Machine> MachineSet { get; set; }

        public DbSet<MachineMessageLog> MachineMessageSet { get; set; }
    }
}
