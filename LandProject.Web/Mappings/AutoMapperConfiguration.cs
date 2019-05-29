using AutoMapper;
using LandProject.Common;
using LandProject.Model.Models;
using LandProject.Web.Models;

namespace LandProject.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();
                cfg.CreateMap<ApplicationRole, ApplicationRoleViewModel>();
                cfg.CreateMap<ApplicationUser, ApplicationUserViewModel>();
                cfg.CreateMap<LandType, LandTypeViewModel>();
                cfg.CreateMap<LandCategory, LandCategoryViewModel>();
                cfg.CreateMap<LProject, LProjectViewModel>();
                cfg.CreateMap<LProjectCategory, LProjectCategoryViewModel>();
                cfg.CreateMap<Province, ProvinceViewModel>();
                cfg.CreateMap<District, DistrictViewModel>();
                cfg.CreateMap<Ward, WardViewModel>();
                cfg.CreateMap<LandNewsFilterViewModel, LandNewsViewModel>();
                cfg.CreateMap<LandFile, LandFileViewModel>();
                cfg.CreateMap<Agent, AgentViewModel>();
                cfg.CreateMap<LandNewsSchedule, LandNewsScheduleViewModel>();
				cfg.CreateMap<Menu, MenuViewModel>();
				cfg.CreateMap<MenuGroup, MenuGroupViewModel>();
				cfg.CreateMap<Notify, NotifyViewModel>();
			});
        }
    }
}