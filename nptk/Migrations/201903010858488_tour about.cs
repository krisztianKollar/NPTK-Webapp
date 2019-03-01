namespace nptk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tourabout : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tours", "About", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tours", "About");
        }
    }
}
