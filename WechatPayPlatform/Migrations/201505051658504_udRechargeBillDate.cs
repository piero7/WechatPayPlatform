namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class udRechargeBillDate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReachargeBills", "UserId", "dbo.Users");
            DropIndex("dbo.ReachargeBills", new[] { "UserId" });
            AlterColumn("dbo.ReachargeBills", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ReachargeBills", "UserId", c => c.Int());
            AddForeignKey("dbo.ReachargeBills", "UserId", "dbo.Users", "UserId");
            CreateIndex("dbo.ReachargeBills", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ReachargeBills", new[] { "UserId" });
            DropForeignKey("dbo.ReachargeBills", "UserId", "dbo.Users");
            AlterColumn("dbo.ReachargeBills", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.ReachargeBills", "CreateDate", c => c.String());
            CreateIndex("dbo.ReachargeBills", "UserId");
            AddForeignKey("dbo.ReachargeBills", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
    }
}
