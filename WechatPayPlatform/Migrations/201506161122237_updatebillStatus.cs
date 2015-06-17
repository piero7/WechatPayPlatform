namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatebillStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MachineBills", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.ShopBills", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShopBills", "Status");
            DropColumn("dbo.MachineBills", "Status");
        }
    }
}
