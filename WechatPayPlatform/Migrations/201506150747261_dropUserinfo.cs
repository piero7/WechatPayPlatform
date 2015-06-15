namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropUserinfo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WechatUsers", "UserInfoId", "dbo.UserInfoes");
            DropForeignKey("dbo.CarInfoes", "UserInfoId", "dbo.UserInfoes");
            DropIndex("dbo.WechatUsers", new[] { "UserInfoId" });
            DropIndex("dbo.CarInfoes", new[] { "UserInfoId" });
            RenameColumn(table: "dbo.CarInfoes", name: "UserInfoId", newName: "UserId");
            AddColumn("dbo.WechatUsers", "Name", c => c.String());
            AddColumn("dbo.WechatUsers", "PhoneNumber", c => c.String());
            AddColumn("dbo.WechatUsers", "StationId", c => c.Int());
            AddForeignKey("dbo.WechatUsers", "StationId", "dbo.Stations", "StationId");
            AddForeignKey("dbo.CarInfoes", "UserId", "dbo.WechatUsers", "UserId");
            CreateIndex("dbo.WechatUsers", "StationId");
            CreateIndex("dbo.CarInfoes", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CarInfoes", new[] { "UserId" });
            DropIndex("dbo.WechatUsers", new[] { "StationId" });
            DropForeignKey("dbo.CarInfoes", "UserId", "dbo.WechatUsers");
            DropForeignKey("dbo.WechatUsers", "StationId", "dbo.Stations");
            DropColumn("dbo.WechatUsers", "StationId");
            DropColumn("dbo.WechatUsers", "PhoneNumber");
            DropColumn("dbo.WechatUsers", "Name");
            RenameColumn(table: "dbo.CarInfoes", name: "UserId", newName: "UserInfoId");
            CreateIndex("dbo.CarInfoes", "UserInfoId");
            CreateIndex("dbo.WechatUsers", "UserInfoId");
            AddForeignKey("dbo.CarInfoes", "UserInfoId", "dbo.UserInfoes", "UserInfoId");
            AddForeignKey("dbo.WechatUsers", "UserInfoId", "dbo.UserInfoes", "UserInfoId");
        }
    }
}
