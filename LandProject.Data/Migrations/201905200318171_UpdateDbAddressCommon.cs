namespace LandProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDbAddressCommon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Provinces", "CountryID", c => c.Int(nullable: false));
            AddColumn("dbo.Provinces", "CountryCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Provinces", "CountryCode");
            DropColumn("dbo.Provinces", "CountryID");
        }
    }
}
