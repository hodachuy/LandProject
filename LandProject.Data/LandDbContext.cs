using Microsoft.AspNet.Identity.EntityFramework;
using LandProject.Model.Models;
using System.Data.Entity;

namespace LandProject.Data
{
    public class LandDbContext : IdentityDbContext<ApplicationUser>
    {
        public LandDbContext() : base("LandDbConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }   
        public DbSet<SystemConfig> SystemConfigs { set; get; }
        public DbSet<VisitorStatistic> VisitorStatistics { set; get; }
        public DbSet<Error> Errors { set; get; }
        public DbSet<Notify> Notifies { set; get; }
        public DbSet<ApplicationGroup> ApplicationGroups { set; get; }
        public DbSet<ApplicationRole> ApplicationRoles { set; get; }
        public DbSet<ApplicationRoleGroup> ApplicationRoleGroups { set; get; }
        public DbSet<ApplicationUserGroup> ApplicationUserGroups { set; get; }
		public DbSet<Agent> Agents { set; get; }
		public DbSet<District> Districts { set; get; }
		public DbSet<Province> Provinces { set; get; }
		public DbSet<Ward> Wards { set; get; }
		public DbSet<LandCategory> LandCategories { set; get; }
		public DbSet<LandNews> LandNewss { set; get; }
		public DbSet<LandFile> LandFiles { set; get; }
		public DbSet<LandNewsOrther> LandNewsOrthers { set; get; }
		public DbSet<LandNewsSchedule> LandNewsSchedules { set; get; }
		public DbSet<LandNewsType> LandNewsTypes { set; get; }
		public DbSet<LProject> LProjects { set; get; }
		public DbSet<LandType> LandTypes { set; get; }
		public DbSet<Post> Posts { set; get; }
		public DbSet<PostCategory> PostCategories { set; get; }
		public DbSet<Tag> Tags { set; get; }
		public DbSet<LandNewsTag> LandNewsTags { set; get; }
		public DbSet<PostTag> PostTags { set; get; }

		public static LandDbContext Create()
        {
            return new LandDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder builder)
        {

            builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("ApplicationUserRoles");
            builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("ApplicationUserLogins");
            builder.Entity<IdentityRole>().ToTable("ApplicationRoles");
            builder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("ApplicationUserClaims");
        }
    }
}
