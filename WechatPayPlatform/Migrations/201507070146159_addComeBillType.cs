namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addComeBillType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComeBillTypes",
                c => new
                    {
                        ComeBillTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Double(),
                        Describe = c.String(),
                        ExplainUrl = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.ComeBillTypeId);
            
            AddColumn("dbo.MachineBills", "PayDate", c => c.DateTime());
            AddColumn("dbo.ComeBills", "ComeBillTypeId", c => c.Int());
            AddColumn("dbo.ComeBills", "PayDate", c => c.DateTime());
            AddColumn("dbo.ShopBills", "PayDate", c => c.DateTime());
            AddForeignKey("dbo.ComeBills", "ComeBillTypeId", "dbo.ComeBillTypes", "ComeBillTypeId");
            CreateIndex("dbo.ComeBills", "ComeBillTypeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ComeBills", new[] { "ComeBillTypeId" });
            DropForeignKey("dbo.ComeBills", "ComeBillTypeId", "dbo.ComeBillTypes");
            DropColumn("dbo.ShopBills", "PayDate");
            DropColumn("dbo.ComeBills", "PayDate");
            DropColumn("dbo.ComeBills", "ComeBillTypeId");
            DropColumn("dbo.MachineBills", "PayDate");
            DropTable("dbo.ComeBillTypes");
        }
    }
}
