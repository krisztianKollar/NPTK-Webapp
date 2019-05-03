namespace nptk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class databasegenerated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pictures", "GalleryID", "dbo.Galleries");
            DropIndex("dbo.Galleries", new[] { "GalleryID" });
            DropPrimaryKey("dbo.Galleries");
            AlterColumn("dbo.Galleries", "GalleryID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Galleries", "GalleryID");
            CreateIndex("dbo.Galleries", "GalleryID");
            AddForeignKey("dbo.Pictures", "GalleryID", "dbo.Galleries", "GalleryID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pictures", "GalleryID", "dbo.Galleries");
            DropIndex("dbo.Galleries", new[] { "GalleryID" });
            DropPrimaryKey("dbo.Galleries");
            AlterColumn("dbo.Galleries", "GalleryID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Galleries", "GalleryID");
            CreateIndex("dbo.Galleries", "GalleryID");
            AddForeignKey("dbo.Pictures", "GalleryID", "dbo.Galleries", "GalleryID", cascadeDelete: true);
        }
    }
}
