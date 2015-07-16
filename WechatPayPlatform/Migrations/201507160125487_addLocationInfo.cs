namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLocationInfo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Administrators", "AreaId", "dbo.Areas");
            DropIndex("dbo.Administrators", new[] { "AreaId" });
            AddColumn("dbo.Stations", "LocationX", c => c.Double());
            AddColumn("dbo.Stations", "LocationY", c => c.Double());
            AddColumn("dbo.Areas", "AdminId", c => c.Int());
            AddColumn("dbo.ComeBills", "LocationX", c => c.Double());
            AddColumn("dbo.ComeBills", "LocationY", c => c.Double());
            AddColumn("dbo.Shops", "ImgPath", c => c.String());
            AddColumn("dbo.Shops", "Minimum", c => c.Double());
            AddForeignKey("dbo.Areas", "AdminId", "dbo.Administrators", "AdministratorId");
            CreateIndex("dbo.Areas", "AdminId");
            DropColumn("dbo.Administrators", "AreaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Administrators", "AreaId", c => c.Int());
            DropIndex("dbo.Areas", new[] { "AdminId" });
            DropForeignKey("dbo.Areas", "AdminId", "dbo.Administrators");
            DropColumn("dbo.Shops", "Minimum");
            DropColumn("dbo.Shops", "ImgPath");
            DropColumn("dbo.ComeBills", "LocationY");
            DropColumn("dbo.ComeBills", "LocationX");
            DropColumn("dbo.Areas", "AdminId");
            DropColumn("dbo.Stations", "LocationY");
            DropColumn("dbo.Stations", "LocationX");
            CreateIndex("dbo.Administrators", "AreaId");
            AddForeignKey("dbo.Administrators", "AreaId", "dbo.Areas", "AreaId");
        }
    }
}
