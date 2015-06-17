namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecarinfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarInfoes", "LastUseDate", c => c.DateTime());
            AddColumn("dbo.CarInfoes", "CreateDate", c => c.DateTime());
            AddColumn("dbo.CarInfoes", "Describe", c => c.String());
            AddColumn("dbo.CarInfoes", "LocationDescribe", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarInfoes", "LocationDescribe");
            DropColumn("dbo.CarInfoes", "Describe");
            DropColumn("dbo.CarInfoes", "CreateDate");
            DropColumn("dbo.CarInfoes", "LastUseDate");
        }
    }
}
