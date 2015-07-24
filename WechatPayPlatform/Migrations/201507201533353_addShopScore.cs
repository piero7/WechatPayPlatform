namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addShopScore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shops", "ScoreId", c => c.Int());
            AddForeignKey("dbo.Shops", "ScoreId", "dbo.Scores", "ScoreId");
            CreateIndex("dbo.Shops", "ScoreId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Shops", new[] { "ScoreId" });
            DropForeignKey("dbo.Shops", "ScoreId", "dbo.Scores");
            DropColumn("dbo.Shops", "ScoreId");
        }
    }
}
