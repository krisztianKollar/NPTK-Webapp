namespace nptk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _double : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tours", "Distance", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tours", "Distance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
