namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWorkHourForStation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stations", "StartWorkHour", c => c.Int());
            AddColumn("dbo.Stations", "EndWorkHour", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stations", "EndWorkHour");
            DropColumn("dbo.Stations", "StartWorkHour");
        }
    }
}
