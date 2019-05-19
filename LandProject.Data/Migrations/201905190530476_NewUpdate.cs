namespace LandProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LProjects", "LProjectCaregoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.LProjects", "LProjectCaregoryID");
            AddForeignKey("dbo.LProjects", "LProjectCaregoryID", "dbo.LProjectCategories", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LProjects", "LProjectCaregoryID", "dbo.LProjectCategories");
            DropIndex("dbo.LProjects", new[] { "LProjectCaregoryID" });
            AlterColumn("dbo.LProjects", "LProjectCaregoryID", c => c.String(nullable: false));
        }
    }
}
