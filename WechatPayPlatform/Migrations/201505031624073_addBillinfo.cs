namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBillinfo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bills", "CreateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bills", "CreateDate", c => c.DateTime(nullable: false));
        }
    }
}
