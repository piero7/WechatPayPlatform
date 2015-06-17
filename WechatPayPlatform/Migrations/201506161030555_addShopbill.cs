namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addShopbill : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShopBills",
                c => new
                    {
                        BillId = c.Int(nullable: false, identity: true),
                        ShopId = c.Int(),
                        Number = c.String(),
                        UserId = c.Int(),
                        CreateDate = c.DateTime(),
                        Count = c.Double(nullable: false),
                        IsSuccess = c.Boolean(nullable: false),
                        innderNumber = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.BillId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .ForeignKey("dbo.WechatUsers", t => t.UserId)
                .Index(t => t.ShopId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ShopBills", new[] { "UserId" });
            DropIndex("dbo.ShopBills", new[] { "ShopId" });
            DropForeignKey("dbo.ShopBills", "UserId", "dbo.WechatUsers");
            DropForeignKey("dbo.ShopBills", "ShopId", "dbo.Shops");
            DropTable("dbo.ShopBills");
        }
    }
}
