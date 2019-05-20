﻿using LandProject.Model.Models;
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

        public static void UpdateLandNews(this LandNews landNews, LandNewsViewModel landNewsVm)
        {
            landNews.ID = landNewsVm.ID;
            landNews.Title = landNewsVm.Title;
            landNews.Alias = landNewsVm.Alias;
            landNews.Description = landNewsVm.Description;
            landNews.Address = landNewsVm.Description;
            landNews.LandTypeID = landNewsVm.LandTypeID;
            landNews.LandCategoryID = landNewsVm.LandCategoryID;
            landNews.ProvinceID = landNewsVm.ProvinceID;
            landNews.DistrictID = landNewsVm.DistrictID;
            landNews.UserID = landNewsVm.UserID;
            landNews.LandNewsScheduleID = landNewsVm.LandNewsScheduleID;
            landNews.WardID = landNewsVm.WardID;
            landNews.LProjectID = landNewsVm.LProjectID;
            landNews.Area = landNewsVm.Area;
            landNews.Price = landNewsVm.Price;
            landNews.TotalPrice = landNewsVm.TotalPrice;
            landNews.Unit = landNewsVm.Unit;
            landNews.Facade = landNewsVm.Facade;
            landNews.Entry = landNewsVm.Entry;
            landNews.CreatedDate = DateTime.Now;
            landNews.MetaKeyword = landNewsVm.MetaKeyword;
            landNews.MetaDescription = landNewsVm.MetaDescription;
            landNews.Status = landNewsVm.Status;
            landNews.HouseDirection = landNewsVm.HouseDirection;
            landNews.BalconyDirection = landNewsVm.BalconyDirection;
            landNews.NumberFloor = landNewsVm.NumberFloor;
            landNews.NumberBedroom = landNewsVm.NumberBedroom;
            landNews.NumberWC = landNewsVm.NumberWC;
            landNews.Furniture = landNewsVm.Furniture;
            landNews.LatiLongTude = landNewsVm.LatiLongTude;
            landNews.IsDelete = landNewsVm.IsDelete;
            landNews.IsPublished = landNewsVm.IsPublished;
            landNews.IsSale = landNewsVm.IsSale;
        }

        public static void UpdateAgent(this Agent agent, AgentViewModel agentVm)
        {
            agent.ID = agentVm.ID;
            agent.Mobile = agentVm.Mobile;
            agent.Name = agentVm.Name;
            agent.Phone = agentVm.Phone;
            agent.Email = agentVm.Email;
            agent.Address = agentVm.Address;
            agent.UserId = agentVm.UserId;
        }

    }
}