namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class udRechargeBillIsSuccess : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReachargeBills", "IsSuccess", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReachargeBills", "IsSuccess");
        }
    }
}
