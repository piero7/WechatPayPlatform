namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRechargeBill : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReachargeBills",
                c => new
                    {
                        RechargeBillId = c.Int(nullable: false, identity: true),
                        OrderId = c.String(),
                        LastStatus = c.String(),
                        CreateDate = c.String(),
                        UserId = c.Int(nullable: true),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.RechargeBillId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ReachargeBills", new[] { "UserId" });
            DropForeignKey("dbo.ReachargeBills", "UserId", "dbo.Users");
            DropTable("dbo.ReachargeBills");
        }
    }
}
