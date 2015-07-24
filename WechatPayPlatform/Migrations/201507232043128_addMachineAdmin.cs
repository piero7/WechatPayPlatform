namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMachineAdmin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MachineAdmins",
                c => new
                    {
                        MachineAdminId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Phone = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.MachineAdminId);
            
            AddColumn("dbo.Machines", "AdminId", c => c.Int());
            AddForeignKey("dbo.Machines", "AdminId", "dbo.MachineAdmins", "MachineAdminId");
            CreateIndex("dbo.Machines", "AdminId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Machines", new[] { "AdminId" });
            DropForeignKey("dbo.Machines", "AdminId", "dbo.MachineAdmins");
            DropColumn("dbo.Machines", "AdminId");
            DropTable("dbo.MachineAdmins");
        }
    }
}
