namespace LandProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LandNewss", "DecimalTotalPrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LProjects", "TotalPrice", c => c.String());
            AddColumn("dbo.LProjects", "DecimalTotalPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LandNewss", "Area", c => c.Int());
            AlterColumn("dbo.LandNewss", "Price", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LandNewss", "Facade", c => c.Int());
            AlterColumn("dbo.LandNewss", "Entry", c => c.Int());
            AlterColumn("dbo.LandNewss", "NumberFloor", c => c.Int());
            AlterColumn("dbo.LandNewss", "NumberBedroom", c => c.Int());
            AlterColumn("dbo.LandNewss", "NumberWC", c => c.Int());
            AlterColumn("dbo.LProjects", "Price", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LProjects", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LandNewss", "NumberWC", c => c.Int(nullable: false));
            AlterColumn("dbo.LandNewss", "NumberBedroom", c => c.Int(nullable: false));
            AlterColumn("dbo.LandNewss", "NumberFloor", c => c.Int(nullable: false));
            AlterColumn("dbo.LandNewss", "Entry", c => c.Int(nullable: false));
            AlterColumn("dbo.LandNewss", "Facade", c => c.Int(nullable: false));
            AlterColumn("dbo.LandNewss", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LandNewss", "Area", c => c.Int(nullable: false));
            DropColumn("dbo.LProjects", "DecimalTotalPrice");
            DropColumn("dbo.LProjects", "TotalPrice");
            DropColumn("dbo.LandNewss", "DecimalTotalPrice");
        }
    }
}
