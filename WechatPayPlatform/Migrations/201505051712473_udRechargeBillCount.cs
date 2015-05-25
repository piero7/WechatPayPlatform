namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class udRechargeBillCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReachargeBills", "Count", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReachargeBills", "Count");
        }
    }
}
