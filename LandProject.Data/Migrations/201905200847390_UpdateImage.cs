namespace LandProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LandNewss", "Image", c => c.String());
            AddColumn("dbo.LProjects", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LProjects", "Image");
            DropColumn("dbo.LandNewss", "Image");
        }
    }
}
