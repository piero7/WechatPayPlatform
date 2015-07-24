namespace WechatPayPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addScore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Scores",
                c => new
                    {
                        ScoreId = c.Int(nullable: false, identity: true),
                        CountA = c.Int(nullable: false, defaultValue: 0),
                        CountB = c.Int(nullable: false, defaultValue: 0),
                        CountC = c.Int(nullable: false, defaultValue: 0),
                        ScoreA = c.Int(nullable: false, defaultValue: 0),
                        ScoreB = c.Int(nullable: false, defaultValue: 0),
                        ScoreC = c.Int(nullable: false, defaultValue: 0),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.ScoreId);

            CreateTable(
                "dbo.ScoreLogs",
                c => new
                    {
                        ScoreLogId = c.Int(nullable: false, identity: true),
                        CreateDate = c.DateTime(),
                        Userid = c.Int(),
                        BillNumber = c.Int(),
                        ScoreId = c.Int(),
                        s1 = c.Int(nullable: false, defaultValue: 0),
                        s2 = c.Int(nullable: false, defaultValue: 0),
                        s3 = c.Int(nullable: false, defaultValue: 0),
                    })
                .PrimaryKey(t => t.ScoreLogId)
                .ForeignKey("dbo.WechatUsers", t => t.Userid)
                .ForeignKey("dbo.Scores", t => t.ScoreId)
                .Index(t => t.Userid)
                .Index(t => t.ScoreId);

        }

        public override void Down()
        {
            DropIndex("dbo.ScoreLogs", new[] { "ScoreId" });
            DropIndex("dbo.ScoreLogs", new[] { "Userid" });
            DropForeignKey("dbo.ScoreLogs", "ScoreId", "dbo.Scores");
            DropForeignKey("dbo.ScoreLogs", "Userid", "dbo.WechatUsers");
            DropTable("dbo.ScoreLogs");
            DropTable("dbo.Scores");
        }
    }
}
