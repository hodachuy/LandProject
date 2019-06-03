namespace LandProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLproject : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LProjects", "Address", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LProjects", "Address", c => c.String(maxLength: 256));
        }
    }
}
