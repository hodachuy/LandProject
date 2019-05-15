namespace LandProject.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        Address = c.String(maxLength: 256),
                        Phone = c.String(maxLength: 50, unicode: false),
                        Mobile = c.String(nullable: false, maxLength: 50, unicode: false),
                        Email = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ApplicationGroups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        Description = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ApplicationRoleGroups",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GroupId, t.RoleId })
                .ForeignKey("dbo.ApplicationGroups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ApplicationRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(maxLength: 250),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.ApplicationRoles", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.ApplicationUserGroups",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.GroupId })
                .ForeignKey("dbo.ApplicationGroups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(maxLength: 256),
                        Address = c.String(maxLength: 256),
                        Avatar = c.String(),
                        MobilePhone = c.String(maxLength: 50, unicode: false),
                        BirthDay = c.DateTime(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserClaims",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Id = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        Type = c.String(maxLength: 50),
                        LatiLongTude = c.String(maxLength: 50),
                        ProvinceID = c.Int(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Errors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        StackTrace = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LandCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Alias = c.String(nullable: false, maxLength: 256, unicode: false),
                        Description = c.String(maxLength: 250),
                        LandTypeID = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LandFiles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 250),
                        Extension = c.String(maxLength: 50, unicode: false),
                        Url = c.String(),
                        LProjectID = c.Int(),
                        LandNewsID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LandNewss",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 256),
                        Alias = c.String(maxLength: 256, unicode: false),
                        Description = c.String(),
                        Address = c.String(nullable: false, maxLength: 256),
                        LandTypeID = c.Int(nullable: false),
                        LandCategoryID = c.Int(nullable: false),
                        ProvinceID = c.Int(nullable: false),
                        DistrictID = c.Int(nullable: false),
                        UserID = c.String(maxLength: 128),
                        LandNewsScheduleID = c.Int(nullable: false),
                        AgentID = c.Int(nullable: false),
                        WardID = c.Int(),
                        LProjectID = c.Int(),
                        Area = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.String(),
                        Unit = c.String(),
                        Facade = c.Int(nullable: false),
                        Entry = c.Int(nullable: false),
                        HouseDirection = c.String(maxLength: 50),
                        BalconyDirection = c.String(maxLength: 50),
                        NumberFloor = c.Int(nullable: false),
                        NumberBedroom = c.Int(nullable: false),
                        NumberWC = c.Int(nullable: false),
                        Furniture = c.String(),
                        LatiLongTude = c.String(maxLength: 50),
                        IsDelete = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        IsSale = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        MetaKeyword = c.String(maxLength: 256),
                        MetaDescription = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LandNewsSchedules",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Alias = c.String(maxLength: 200),
                        Description = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LandNewsTags",
                c => new
                    {
                        LandNewsID = c.Int(nullable: false),
                        TagID = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.LandNewsID, t.TagID })
                .ForeignKey("dbo.LandNewss", t => t.LandNewsID, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagID, cascadeDelete: true)
                .Index(t => t.LandNewsID)
                .Index(t => t.TagID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Type = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LandTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Alias = c.String(nullable: false, maxLength: 256, unicode: false),
                        Description = c.String(maxLength: 250),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LProjectCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Alias = c.String(nullable: false, maxLength: 256, unicode: false),
                        Description = c.String(maxLength: 250),
                        IsDelete = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LProjects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Alias = c.String(nullable: false, maxLength: 256, unicode: false),
                        Address = c.String(maxLength: 256),
                        Description = c.String(maxLength: 250),
                        Investors = c.String(),
                        LProjectCaregoryID = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Unit = c.String(),
                        Area = c.String(),
                        Size = c.String(),
                        ProvinceID = c.Int(nullable: false),
                        DistrictID = c.Int(nullable: false),
                        WardID = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        MetaKeyword = c.String(maxLength: 256),
                        MetaDescription = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Notifies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(maxLength: 256),
                        SenderID = c.Int(),
                        RecipientID = c.Int(),
                        IsRead = c.Boolean(),
                        TableName = c.String(maxLength: 50),
                        TableItemID = c.Int(),
                        CreatedDate = c.DateTime(),
                        URL = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PostCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Alias = c.String(nullable: false, maxLength: 256, unicode: false),
                        Description = c.String(maxLength: 500),
                        ParentID = c.Int(),
                        DisplayOrder = c.Int(),
                        Image = c.String(maxLength: 256),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        MetaKeyword = c.String(maxLength: 256),
                        MetaDescription = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Alias = c.String(nullable: false, maxLength: 256, unicode: false),
                        PostCategoryID = c.Int(nullable: false),
                        Image = c.String(maxLength: 256),
                        Description = c.String(maxLength: 500),
                        Content = c.String(),
                        IsPublished = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        ViewCount = c.Int(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        MetaKeyword = c.String(maxLength: 256),
                        MetaDescription = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PostCategories", t => t.PostCategoryID, cascadeDelete: true)
                .Index(t => t.PostCategoryID);
            
            CreateTable(
                "dbo.PostTags",
                c => new
                    {
                        PostID = c.Int(nullable: false),
                        TagID = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.PostID, t.TagID })
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagID, cascadeDelete: true)
                .Index(t => t.PostID)
                .Index(t => t.TagID);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Type = c.String(maxLength: 20),
                        TelephoneCode = c.Int(nullable: false),
                        ZipCode = c.String(maxLength: 20),
                        SortOrder = c.Int(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SystemConfigs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50, unicode: false),
                        ValueString = c.String(maxLength: 50),
                        ValueInt = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.VisitorStatistics",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        VisitedDate = c.DateTime(nullable: false),
                        IPAddress = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Wards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Type = c.String(maxLength: 50),
                        LatiLongTude = c.String(maxLength: 50),
                        DistrictID = c.Int(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserRoles", "IdentityRole_Id", "dbo.ApplicationRoles");
            DropForeignKey("dbo.PostTags", "TagID", "dbo.Tags");
            DropForeignKey("dbo.PostTags", "PostID", "dbo.Posts");
            DropForeignKey("dbo.Posts", "PostCategoryID", "dbo.PostCategories");
            DropForeignKey("dbo.LandNewsTags", "TagID", "dbo.Tags");
            DropForeignKey("dbo.LandNewsTags", "LandNewsID", "dbo.LandNewss");
            DropForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.ApplicationRoles");
            DropForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups");
            DropIndex("dbo.PostTags", new[] { "TagID" });
            DropIndex("dbo.PostTags", new[] { "PostID" });
            DropIndex("dbo.Posts", new[] { "PostCategoryID" });
            DropIndex("dbo.LandNewsTags", new[] { "TagID" });
            DropIndex("dbo.LandNewsTags", new[] { "LandNewsID" });
            DropIndex("dbo.ApplicationUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "GroupId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "UserId" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "RoleId" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "GroupId" });
            DropTable("dbo.Wards");
            DropTable("dbo.VisitorStatistics");
            DropTable("dbo.SystemConfigs");
            DropTable("dbo.Provinces");
            DropTable("dbo.PostTags");
            DropTable("dbo.Posts");
            DropTable("dbo.PostCategories");
            DropTable("dbo.Notifies");
            DropTable("dbo.LProjects");
            DropTable("dbo.LProjectCategories");
            DropTable("dbo.LandTypes");
            DropTable("dbo.Tags");
            DropTable("dbo.LandNewsTags");
            DropTable("dbo.LandNewsSchedules");
            DropTable("dbo.LandNewss");
            DropTable("dbo.LandFiles");
            DropTable("dbo.LandCategories");
            DropTable("dbo.Errors");
            DropTable("dbo.Districts");
            DropTable("dbo.ApplicationUserLogins");
            DropTable("dbo.ApplicationUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.ApplicationUserGroups");
            DropTable("dbo.ApplicationUserRoles");
            DropTable("dbo.ApplicationRoles");
            DropTable("dbo.ApplicationRoleGroups");
            DropTable("dbo.ApplicationGroups");
            DropTable("dbo.Agents");
        }
    }
}
