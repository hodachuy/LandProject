using LandProject.Model.Models;
using LandProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using LandProject.Common;
using System.Text.RegularExpressions;

namespace LandProject.Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
		public static void UpdateLandType(this LandType landType, LandTypeViewModel landTypeVm)
		{
			landType.ID = landTypeVm.ID;
			landType.Name = landTypeVm.Name;
			landType.Description = landTypeVm.Description;
			landType.Alias = landTypeVm.Alias;
			landType.IsDelete = landTypeVm.IsDelete;
		}

		public static void UpdateLandCategory(this LandCategory landCategory, LandCategoryViewModel landCategoryVm)
		{
			landCategory.ID = landCategoryVm.ID;
			landCategory.Name = landCategoryVm.Name;
			landCategory.Description = landCategoryVm.Description;
			landCategory.Alias = landCategoryVm.Alias;
			landCategory.IsDelete = landCategoryVm.IsDelete;
			landCategory.LandTypeID = landCategoryVm.LandTypeID;
		}

		public static void UpdateLProjectCategory(this LProjectCategory lProjectCategory, LProjectCategoryViewModel lProjectCategoryVm)
		{
			lProjectCategory.ID = lProjectCategoryVm.ID;
			lProjectCategory.Name = lProjectCategoryVm.Name;
			lProjectCategory.Description = lProjectCategoryVm.Description;
			lProjectCategory.Alias = lProjectCategoryVm.Alias;
			lProjectCategory.IsDelete = lProjectCategoryVm.IsDelete;
		}
	}
}