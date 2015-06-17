namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatebillremarks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MachineBills", "Remarks", c => c.String());
            DropColumn("dbo.MachineBills", "Remaeks");
            DropColumn("dbo.ComeBills", "Remaeks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ComeBills", "Remaeks", c => c.String());
            AddColumn("dbo.MachineBills", "Remaeks", c => c.String());
            DropColumn("dbo.MachineBills", "Remarks");
        }
    }
}
