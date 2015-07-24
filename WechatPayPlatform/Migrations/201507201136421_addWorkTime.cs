namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWorkTime : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkTimes",
                c => new
                    {
                        WorkTimeId = c.Int(nullable: false, identity: true),
                        StationId = c.Int(),
                        LastEditTime = c.DateTime(),
                        Content = c.String(),
                        Remarkes = c.String(),
                    })
                .PrimaryKey(t => t.WorkTimeId)
                .ForeignKey("dbo.Stations", t => t.StationId)
                .Index(t => t.StationId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkTimes", new[] { "StationId" });
            DropForeignKey("dbo.WorkTimes", "StationId", "dbo.Stations");
            DropTable("dbo.WorkTimes");
        }
    }
}
