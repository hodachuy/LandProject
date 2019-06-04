namespace LandProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLandTypeExchange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LandTypes", "TypeExchange", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LandTypes", "TypeExchange");
        }
    }
}
