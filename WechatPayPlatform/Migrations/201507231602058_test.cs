namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Scores", "ScoreA", c => c.Double(nullable: false));
            AlterColumn("dbo.Scores", "ScoreB", c => c.Double(nullable: false));
            AlterColumn("dbo.Scores", "ScoreC", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Scores", "ScoreC", c => c.Int(nullable: false));
            AlterColumn("dbo.Scores", "ScoreB", c => c.Int(nullable: false));
            AlterColumn("dbo.Scores", "ScoreA", c => c.Int(nullable: false));
        }
    }
}
