namespace nptk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tourbools : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tours", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tours", "IsActual", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tours", "IsActual");
            DropColumn("dbo.Tours", "IsActive");
        }
    }
}
