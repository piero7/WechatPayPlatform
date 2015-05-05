namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addbill : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        BillId = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        UserId = c.Int(),
                        CreateDate = c.DateTime(nullable: true),
                        Count = c.Double(nullable: false,defaultValue:0),
                        IsSuccess = c.Boolean(nullable: false,defaultValue:false),
                        Remaeks = c.String(),
                    })
                .PrimaryKey(t => t.BillId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Bills", new[] { "UserId" });
            DropForeignKey("dbo.Bills", "UserId", "dbo.Users");
            DropTable("dbo.Bills");
        }
    }
}
