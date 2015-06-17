namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addShopname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shops", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shops", "Name");
        }
    }
}
