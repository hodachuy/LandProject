namespace LandProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAgenPhone2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Agents", "Phone2", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Agents", "Phone3", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Agents", "PhoneShow", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Agents", "PhoneShow", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Agents", "Phone3", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Agents", "Phone2", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
    }
}
