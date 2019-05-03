namespace nptk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Galleries1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Galleries", "TourID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Galleries", "TourID");
        }
    }
}
