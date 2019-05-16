namespace LandProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKLT : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.LandCategories", "LandTypeID");
            AddForeignKey("dbo.LandCategories", "LandTypeID", "dbo.LandTypes", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LandCategories", "LandTypeID", "dbo.LandTypes");
            DropIndex("dbo.LandCategories", new[] { "LandTypeID" });
        }
    }
}
