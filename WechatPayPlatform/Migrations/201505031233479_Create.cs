namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        OpenId = c.String(),
                        NickName = c.String(),
                        subscribe = c.Boolean(nullable: true, defaultValue: false),
                        Sex = c.Int(nullable: true, defaultValue: 0),
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
                .PrimaryKey(t => t.CodeId)
                .ForeignKey("dbo.Machines", t => t.MachineId)
                .Index(t => t.MachineId);

            CreateTable(
                "dbo.Machines",
                c => new
                    {
                        MachineId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        InnerId = c.String(),
                        Address = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.MachineId);

        }

        public override void Down()
        {
            DropIndex("dbo.Codes", new[] { "MachineId" });
            DropForeignKey("dbo.Codes", "MachineId", "dbo.Machines");
            DropTable("dbo.Machines");
            DropTable("dbo.Codes");
            DropTable("dbo.Users");
        }
    }
}
