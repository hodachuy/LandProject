namespace LandProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAgentPhone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agents", "Phone1", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.Agents", "Phone2", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AddColumn("dbo.Agents", "Phone3", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AddColumn("dbo.Agents", "PhoneShow", c => c.String(nullable: false, maxLength: 50, unicode: false));
            DropColumn("dbo.Agents", "Phone");
            DropColumn("dbo.Agents", "Mobile");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Agents", "Mobile", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AddColumn("dbo.Agents", "Phone", c => c.String(maxLength: 50, unicode: false));
            DropColumn("dbo.Agents", "PhoneShow");
            DropColumn("dbo.Agents", "Phone3");
            DropColumn("dbo.Agents", "Phone2");
            DropColumn("dbo.Agents", "Phone1");
        }
    }
}
