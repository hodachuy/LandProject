namespace LandProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAddressCommon : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Districts", "ProvinceID");
            CreateIndex("dbo.Wards", "DistrictID");
            AddForeignKey("dbo.Districts", "ProvinceID", "dbo.Provinces", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Wards", "DistrictID", "dbo.Districts", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Wards", "DistrictID", "dbo.Districts");
            DropForeignKey("dbo.Districts", "ProvinceID", "dbo.Provinces");
            DropIndex("dbo.Wards", new[] { "DistrictID" });
            DropIndex("dbo.Districts", new[] { "ProvinceID" });
        }
    }
}
