namespace nptk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedpicmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        PicID = c.Int(nullable: false, identity: true),
                        TourID = c.Int(nullable: false),
                        Path = c.String(),
                        PicName = c.String(),
                    })
                .PrimaryKey(t => t.PicID)
                .ForeignKey("dbo.Tours", t => t.TourID, cascadeDelete: true)
                .Index(t => t.TourID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pictures", "TourID", "dbo.Tours");
            DropIndex("dbo.Pictures", new[] { "TourID" });
            DropTable("dbo.Pictures");
        }
    }
}
