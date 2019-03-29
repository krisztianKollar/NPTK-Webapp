namespace nptk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reupdatenews : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.News",
                c => new
                    {
                        NewsId = c.Int(nullable: false, identity: true),
                        NewsTitle = c.String(),
                        NewsDate = c.DateTime(nullable: false),
                        NewsAbout = c.String(),
                    })
                .PrimaryKey(t => t.NewsId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.News");
        }
    }
}
