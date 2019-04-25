namespace nptk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PicGallery : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pictures", "TourID", "dbo.Tours");
            DropForeignKey("dbo.Pictures", "Gallery_GalleryID", "dbo.Galleries");
            DropForeignKey("dbo.Galleries", "TourID", "dbo.Tours");
            DropIndex("dbo.Galleries", new[] { "TourID" });
            DropIndex("dbo.Pictures", new[] { "TourID" });
            DropIndex("dbo.Pictures", new[] { "Gallery_GalleryID" });
            CreateTable(
                "dbo.PicGalleries",
                c => new
                    {
                        GalleryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GalleryID)
                .ForeignKey("dbo.Tours", t => t.GalleryID)
                .Index(t => t.GalleryID);
            
            CreateTable(
                "dbo.TourPictures",
                c => new
                    {
                        PicID = c.Int(nullable: false, identity: true),
                        GalleryID = c.Int(nullable: false),
                        Path = c.String(),
                        PicName = c.String(),
                    })
                .PrimaryKey(t => t.PicID)
                .ForeignKey("dbo.PicGalleries", t => t.GalleryID, cascadeDelete: true)
                .Index(t => t.GalleryID);
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        PicID = c.Int(nullable: false, identity: true),
                        TourID = c.Int(nullable: false),
                        Path = c.String(),
                        PicName = c.String(),
                        Gallery_GalleryID = c.Int(),
                    })
                .PrimaryKey(t => t.PicID);
            
            CreateTable(
                "dbo.Galleries",
                c => new
                    {
                        GalleryID = c.Int(nullable: false, identity: true),
                        TourID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GalleryID);
            
            DropForeignKey("dbo.TourPictures", "GalleryID", "dbo.PicGalleries");
            DropForeignKey("dbo.PicGalleries", "GalleryID", "dbo.Tours");
            DropIndex("dbo.TourPictures", new[] { "GalleryID" });
            DropIndex("dbo.PicGalleries", new[] { "GalleryID" });
            DropTable("dbo.TourPictures");
            DropTable("dbo.PicGalleries");
            CreateIndex("dbo.Pictures", "Gallery_GalleryID");
            CreateIndex("dbo.Pictures", "TourID");
            CreateIndex("dbo.Galleries", "TourID");
            AddForeignKey("dbo.Galleries", "TourID", "dbo.Tours", "TourId", cascadeDelete: true);
            AddForeignKey("dbo.Pictures", "Gallery_GalleryID", "dbo.Galleries", "GalleryID");
            AddForeignKey("dbo.Pictures", "TourID", "dbo.Tours", "TourId", cascadeDelete: true);
        }
    }
}
