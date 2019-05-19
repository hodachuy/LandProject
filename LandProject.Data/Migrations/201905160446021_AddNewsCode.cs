namespace LandProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class f : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LandNewss", "Code", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LandNewss", "Code");
        }
    }
}
