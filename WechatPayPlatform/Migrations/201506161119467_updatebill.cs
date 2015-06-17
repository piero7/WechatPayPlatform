namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatebill : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MachineBills", "innerNumber", c => c.String());
            AddColumn("dbo.ComeBills", "innerNumber", c => c.String());
            AddColumn("dbo.ShopBills", "innerNumber", c => c.String());
            DropColumn("dbo.MachineBills", "innderNumber");
            DropColumn("dbo.ComeBills", "innderNumber");
            DropColumn("dbo.ShopBills", "innderNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShopBills", "innderNumber", c => c.String());
            AddColumn("dbo.ComeBills", "innderNumber", c => c.String());
            AddColumn("dbo.MachineBills", "innderNumber", c => c.String());
            DropColumn("dbo.ShopBills", "innerNumber");
            DropColumn("dbo.ComeBills", "innerNumber");
            DropColumn("dbo.MachineBills", "innerNumber");
        }
    }
}
