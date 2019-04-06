namespace nptk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class picturemodel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tours", "PosterPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tours", "PosterPath", c => c.String());
        }
    }
}
