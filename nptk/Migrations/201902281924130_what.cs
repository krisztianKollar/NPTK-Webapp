namespace nptk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class what : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tours", "IsActive");
            DropColumn("dbo.Tours", "IsActual");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tours", "IsActual", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tours", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
