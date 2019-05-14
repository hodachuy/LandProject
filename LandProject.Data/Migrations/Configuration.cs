﻿namespace LandProject.Data.Migrations
{
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;
	using Model.Models;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<LandProject.Data.LandDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
		}

		protected override void Seed(LandProject.Data.LandDbContext context)
		{
			//  This method will be called after migrating to the latest version.
			//CreateProductCategorySample(context);
			//CreateSlide(context);
			//ContactDetail(context);

			CreateUser(context);
		}

		private void CreateUser(LandDbContext context)
		{
			var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new LandDbContext()));

			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new LandDbContext()));

			var user = new ApplicationUser()
			{
				UserName = "huyho",
				Email = "hodachuy123@gmail.com",
				EmailConfirmed = true,
				BirthDay = Convert.ToDateTime("06/06/1991"),
				FullName = "Ho Dac Huy"
			};

			manager.Create(user, "hdhuy@123456");

			if (!roleManager.Roles.Any())
			{
				roleManager.Create(new IdentityRole { Name = "Admin" });
				roleManager.Create(new IdentityRole { Name = "User" });
			}

			var adminUser = manager.FindByEmail("hodachuy123@gmail.com");

			manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
		}


	}
}
