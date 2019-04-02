namespace nptk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class poster : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tours", "PosterPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tours", "PosterPath");
        }
    }
}
