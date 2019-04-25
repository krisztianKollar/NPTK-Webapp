namespace nptk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gallery : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PicGalleries", newName: "Galleries");
            RenameTable(name: "dbo.TourPictures", newName: "Pictures");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Pictures", newName: "TourPictures");
            RenameTable(name: "dbo.Galleries", newName: "PicGalleries");
        }
    }
}
