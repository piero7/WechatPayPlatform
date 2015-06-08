namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addMachine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MachineMessageLogs",
                c => new
                    {
                        MachineMessageLogId = c.Int(nullable: false, identity: true),
                        Messgae = c.String(),
                        CreateDate = c.DateTime(),
                        MessageType = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MachineMessageLogId);

            AddColumn("dbo.Machines", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Machines", "IpAddress", c => c.String());
            AddColumn("dbo.Machines", "IsActive", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.Machines", "LastConnectedTime", c => c.DateTime());
            AddColumn("dbo.Bills", "MachineId", c => c.Int());
            AddColumn("dbo.Bills", "innderNumber", c => c.String());
            AddForeignKey("dbo.Bills", "MachineId", "dbo.Machines", "MachineId");
            CreateIndex("dbo.Bills", "MachineId");
        }

        public override void Down()
        {
            DropIndex("dbo.Bills", new[] { "MachineId" });
            DropForeignKey("dbo.Bills", "MachineId", "dbo.Machines");
            DropColumn("dbo.Bills", "innderNumber");
            DropColumn("dbo.Bills", "MachineId");
            DropColumn("dbo.Machines", "LastConnectedTime");
            DropColumn("dbo.Machines", "IsActive");
            DropColumn("dbo.Machines", "IpAddress");
            DropColumn("dbo.Machines", "Type");
            DropTable("dbo.MachineMessageLogs");
        }
    }
}
