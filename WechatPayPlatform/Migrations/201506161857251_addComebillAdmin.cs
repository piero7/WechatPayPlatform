namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addComebillAdmin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComeBills", "AdminId", c => c.Int());
            AddForeignKey("dbo.ComeBills", "AdminId", "dbo.Administrators", "AdministratorId");
            CreateIndex("dbo.ComeBills", "AdminId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ComeBills", new[] { "AdminId" });
            DropForeignKey("dbo.ComeBills", "AdminId", "dbo.Administrators");
            DropColumn("dbo.ComeBills", "AdminId");
        }
    }
}
