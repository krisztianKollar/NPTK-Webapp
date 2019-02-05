namespace nptk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hikers",
                c => new
                    {
                        HikerID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        LastName = c.String(),
                        FirstName = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.HikerID);
            
            CreateTable(
                "dbo.SignUps",
                c => new
                    {
                        SignUpID = c.Int(nullable: false, identity: true),
                        TourID = c.Int(nullable: false),
                        HikerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SignUpID)
                .ForeignKey("dbo.Hikers", t => t.HikerID, cascadeDelete: true)
                .ForeignKey("dbo.Tours", t => t.TourID, cascadeDelete: true)
                .Index(t => t.TourID)
                .Index(t => t.HikerID);
            
            CreateTable(
                "dbo.Tours",
                c => new
                    {
                        TourId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Date = c.DateTime(nullable: false),
                        Track = c.String(),
                        Distance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Climb = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TourId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SignUps", "TourID", "dbo.Tours");
            DropForeignKey("dbo.SignUps", "HikerID", "dbo.Hikers");
            DropIndex("dbo.SignUps", new[] { "HikerID" });
            DropIndex("dbo.SignUps", new[] { "TourID" });
            DropTable("dbo.Tours");
            DropTable("dbo.SignUps");
            DropTable("dbo.Hikers");
        }
    }
}
