namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAdminAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Administrators", "Account", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Administrators", "Account");
        }
    }
}
