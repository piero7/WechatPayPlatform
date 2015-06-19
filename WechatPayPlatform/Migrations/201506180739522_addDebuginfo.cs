namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDebuginfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DebugInfoes",
                c => new
                    {
                        InfoId = c.Int(nullable: false, identity: true),
                        Info = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.InfoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DebugInfoes");
        }
    }
}
