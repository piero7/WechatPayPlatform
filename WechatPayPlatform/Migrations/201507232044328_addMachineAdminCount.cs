namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMachineAdminCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MachineAdmins", "Count", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MachineAdmins", "Count");
        }
    }
}
