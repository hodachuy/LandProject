namespace LandProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLandFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LandFiles", "PostID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LandFiles", "PostID");
        }
    }
}
