namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToken : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessTokens",
                c => new
                    {
                        AcceccTokenId = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        GetTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.AcceccTokenId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccessTokens");
        }
    }
}
