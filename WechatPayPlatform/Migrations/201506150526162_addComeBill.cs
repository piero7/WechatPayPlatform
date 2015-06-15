namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addComeBill : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Codes", "MachineId", "dbo.Machines");
            DropForeignKey("dbo.Bills", "UserId", "dbo.Users");
            DropForeignKey("dbo.Bills", "MachineId", "dbo.Machines");
            DropForeignKey("dbo.ReachargeBills", "UserId", "dbo.Users");
            DropIndex("dbo.Codes", new[] { "MachineId" });
            DropIndex("dbo.Bills", new[] { "UserId" });
            DropIndex("dbo.Bills", new[] { "MachineId" });
            DropIndex("dbo.ReachargeBills", new[] { "UserId" });
            CreateTable(
                "dbo.WechatUsers",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        OpenId = c.String(),
                        NickName = c.String(),
                        subscribe = c.Boolean(nullable: false),
                        Sex = c.Int(nullable: false),
                        City = c.String(),
                        Country = c.String(),
                        Province = c.String(),
                        Language = c.String(),
                        SubscribeTime = c.DateTime(),
                        Headimgurl = c.String(),
                        Balance = c.Double(),
                        Remarks = c.String(),
                        UserInfoId = c.Int(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfoId)
                .Index(t => t.UserInfoId);
            
            CreateTable(
                "dbo.UserInfoes",
                c => new
                    {
                        UserInfoId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PhoneNumber = c.String(),
                        StationId = c.Int(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.UserInfoId)
                .ForeignKey("dbo.Stations", t => t.StationId)
                .Index(t => t.StationId);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        StationId = c.Int(nullable: false, identity: true),
                        InnerNumber = c.String(),
                        AreaId = c.Int(),
                        Name = c.String(),
                        AdministratorId = c.Int(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.StationId)
                .ForeignKey("dbo.Areas", t => t.AreaId)
                .ForeignKey("dbo.Administrators", t => t.AdministratorId)
                .Index(t => t.AreaId)
                .Index(t => t.AdministratorId);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        AreaId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Discribe = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.AreaId);
            
            CreateTable(
                "dbo.Administrators",
                c => new
                    {
                        AdministratorId = c.Int(nullable: false, identity: true),
                        AreaId = c.Int(),
                        Name = c.String(),
                        PhoneNumber = c.String(),
                        IdNumber = c.String(),
                        UserId = c.Int(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.AdministratorId)
                .ForeignKey("dbo.Areas", t => t.AreaId)
                .ForeignKey("dbo.WechatUsers", t => t.UserId)
                .Index(t => t.AreaId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MachineCodes",
                c => new
                    {
                        CodeId = c.Int(nullable: false, identity: true),
                        MachineId = c.Int(),
                        Url = c.String(),
                        Content = c.String(),
                        EventKey = c.String(),
                    })
                .PrimaryKey(t => t.CodeId)
                .ForeignKey("dbo.Machines", t => t.MachineId)
                .Index(t => t.MachineId);
            
            CreateTable(
                "dbo.StationCodes",
                c => new
                    {
                        CodeId = c.Int(nullable: false, identity: true),
                        StationId = c.Int(),
                        Url = c.String(),
                        Content = c.String(),
                        EventKey = c.String(),
                    })
                .PrimaryKey(t => t.CodeId)
                .ForeignKey("dbo.Stations", t => t.StationId)
                .Index(t => t.StationId);
            
            CreateTable(
                "dbo.MachineBills",
                c => new
                    {
                        BillId = c.Int(nullable: false, identity: true),
                        MachineId = c.Int(),
                        Number = c.String(),
                        UserId = c.Int(),
                        CreateDate = c.DateTime(),
                        Count = c.Double(nullable: false),
                        IsSuccess = c.Boolean(nullable: false),
                        innderNumber = c.String(),
                        Remaeks = c.String(),
                    })
                .PrimaryKey(t => t.BillId)
                .ForeignKey("dbo.Machines", t => t.MachineId)
                .ForeignKey("dbo.WechatUsers", t => t.UserId)
                .Index(t => t.MachineId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ComeBills",
                c => new
                    {
                        BillId = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        PhoneNumber = c.String(),
                        CarNumber = c.String(),
                        CarInfo = c.String(),
                        Address = c.String(),
                        Describe = c.String(),
                        Remarks = c.String(),
                        FinishTime = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Number = c.String(),
                        UserId = c.Int(),
                        CreateDate = c.DateTime(),
                        Count = c.Double(nullable: false),
                        IsSuccess = c.Boolean(nullable: false),
                        innderNumber = c.String(),
                        Remaeks = c.String(),
                    })
                .PrimaryKey(t => t.BillId)
                .ForeignKey("dbo.WechatUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Neighborhoods",
                c => new
                    {
                        NeighborhoodId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        StationId = c.Int(),
                        Reamarks = c.String(),
                    })
                .PrimaryKey(t => t.NeighborhoodId)
                .ForeignKey("dbo.Stations", t => t.StationId)
                .Index(t => t.StationId);
            
            CreateTable(
                "dbo.CarInfoes",
                c => new
                    {
                        CarId = c.Int(nullable: false, identity: true),
                        UserInfoId = c.Int(),
                        LocationX = c.Double(),
                        LocationY = c.Double(),
                        CarNumber = c.String(),
                        NeighborhoodId = c.Int(),
                    })
                .PrimaryKey(t => t.CarId)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfoId)
                .ForeignKey("dbo.Neighborhoods", t => t.NeighborhoodId)
                .Index(t => t.UserInfoId)
                .Index(t => t.NeighborhoodId);
            
            CreateTable(
                "dbo.Shops",
                c => new
                    {
                        ShopId = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        LocationX = c.Double(),
                        LocationY = c.Double(),
                        Phone = c.String(),
                        AdminName = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.ShopId);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        PackaheId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShopId = c.Int(),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.PackaheId)
                .ForeignKey("dbo.Shops", t => t.ShopId)
                .Index(t => t.ShopId);
            
            AddForeignKey("dbo.ReachargeBills", "UserId", "dbo.WechatUsers", "UserId");
            CreateIndex("dbo.ReachargeBills", "UserId");
            DropTable("dbo.Users");
            DropTable("dbo.Codes");
            DropTable("dbo.Bills");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        BillId = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        UserId = c.Int(),
                        CreateDate = c.DateTime(),
                        Count = c.Double(nullable: false),
                        IsSuccess = c.Boolean(nullable: false),
                        MachineId = c.Int(),
                        innderNumber = c.String(),
                        Remaeks = c.String(),
                    })
                .PrimaryKey(t => t.BillId);
            
            CreateTable(
                "dbo.Codes",
                c => new
                    {
                        CodeId = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        Content = c.String(),
                        EventKey = c.String(),
                        MachineId = c.Int(),
                    })
                .PrimaryKey(t => t.CodeId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        OpenId = c.String(),
                        NickName = c.String(),
                        subscribe = c.Boolean(nullable: false),
                        Sex = c.Int(nullable: false),
                        City = c.String(),
                        Country = c.String(),
                        Province = c.String(),
                        Language = c.String(),
                        SubscribeTime = c.DateTime(),
                        Headimgurl = c.String(),
                        Balance = c.Double(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            DropIndex("dbo.Packages", new[] { "ShopId" });
            DropIndex("dbo.CarInfoes", new[] { "NeighborhoodId" });
            DropIndex("dbo.CarInfoes", new[] { "UserInfoId" });
            DropIndex("dbo.Neighborhoods", new[] { "StationId" });
            DropIndex("dbo.ReachargeBills", new[] { "UserId" });
            DropIndex("dbo.ComeBills", new[] { "UserId" });
            DropIndex("dbo.MachineBills", new[] { "UserId" });
            DropIndex("dbo.MachineBills", new[] { "MachineId" });
            DropIndex("dbo.StationCodes", new[] { "StationId" });
            DropIndex("dbo.MachineCodes", new[] { "MachineId" });
            DropIndex("dbo.Administrators", new[] { "UserId" });
            DropIndex("dbo.Administrators", new[] { "AreaId" });
            DropIndex("dbo.Stations", new[] { "AdministratorId" });
            DropIndex("dbo.Stations", new[] { "AreaId" });
            DropIndex("dbo.UserInfoes", new[] { "StationId" });
            DropIndex("dbo.WechatUsers", new[] { "UserInfoId" });
            DropForeignKey("dbo.Packages", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.CarInfoes", "NeighborhoodId", "dbo.Neighborhoods");
            DropForeignKey("dbo.CarInfoes", "UserInfoId", "dbo.UserInfoes");
            DropForeignKey("dbo.Neighborhoods", "StationId", "dbo.Stations");
            DropForeignKey("dbo.ReachargeBills", "UserId", "dbo.WechatUsers");
            DropForeignKey("dbo.ComeBills", "UserId", "dbo.WechatUsers");
            DropForeignKey("dbo.MachineBills", "UserId", "dbo.WechatUsers");
            DropForeignKey("dbo.MachineBills", "MachineId", "dbo.Machines");
            DropForeignKey("dbo.StationCodes", "StationId", "dbo.Stations");
            DropForeignKey("dbo.MachineCodes", "MachineId", "dbo.Machines");
            DropForeignKey("dbo.Administrators", "UserId", "dbo.WechatUsers");
            DropForeignKey("dbo.Administrators", "AreaId", "dbo.Areas");
            DropForeignKey("dbo.Stations", "AdministratorId", "dbo.Administrators");
            DropForeignKey("dbo.Stations", "AreaId", "dbo.Areas");
            DropForeignKey("dbo.UserInfoes", "StationId", "dbo.Stations");
            DropForeignKey("dbo.WechatUsers", "UserInfoId", "dbo.UserInfoes");
            DropTable("dbo.Packages");
            DropTable("dbo.Shops");
            DropTable("dbo.CarInfoes");
            DropTable("dbo.Neighborhoods");
            DropTable("dbo.ComeBills");
            DropTable("dbo.MachineBills");
            DropTable("dbo.StationCodes");
            DropTable("dbo.MachineCodes");
            DropTable("dbo.Administrators");
            DropTable("dbo.Areas");
            DropTable("dbo.Stations");
            DropTable("dbo.UserInfoes");
            DropTable("dbo.WechatUsers");
            CreateIndex("dbo.ReachargeBills", "UserId");
            CreateIndex("dbo.Bills", "MachineId");
            CreateIndex("dbo.Bills", "UserId");
            CreateIndex("dbo.Codes", "MachineId");
            AddForeignKey("dbo.ReachargeBills", "UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Bills", "MachineId", "dbo.Machines", "MachineId");
            AddForeignKey("dbo.Bills", "UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Codes", "MachineId", "dbo.Machines", "MachineId");
        }
    }
}
