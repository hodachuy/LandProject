using LandProject.Data.Infrastructure;
using LandProject.Data.Repositories;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Service
{
	public interface IMenuGroupService
	{
		IEnumerable<MenuGroup> GetMenuGroup();
        IEnumerable<MenuGroup> GetMenuGroupActive();
        IEnumerable<MenuGroup> GetAll();
		IEnumerable<MenuGroup> GetAll(string keyword);
		MenuGroup Add(MenuGroup menuGroup);
		void Update(MenuGroup menuGroup);
		MenuGroup Delete(int id);
		MenuGroup GetByIdMenuGroup(int id);
		void Save();
	}
	public class MenuGroupService : IMenuGroupService
	{
		IMenuGroupRepository _menuGroupRepository;
		IUnitOfWork _unitOfWork;
		public MenuGroupService(IMenuGroupRepository menuGroupRepository, IUnitOfWork unitOfWork)
		{
			_menuGroupRepository = menuGroupRepository;
			_unitOfWork = unitOfWork;
		}

		public IEnumerable<MenuGroup> GetMenuGroup()
		{
			return _menuGroupRepository.GetMulti(x => x.Status);
		}

		public MenuGroup Add(MenuGroup menuGroup)
		{
			return _menuGroupRepository.Add(menuGroup);
		}

		public void Update(MenuGroup menuGroup)
		{
			_menuGroupRepository.Update(menuGroup);
		}

		public MenuGroup Delete(int id)
		{
			return _menuGroupRepository.Delete(id);
		}

		public MenuGroup GetByIdMenuGroup(int id)
		{
			return _menuGroupRepository.GetSingleById(id);
		}
		public void Save()
		{
			_unitOfWork.Commit();
		}

		public IEnumerable<MenuGroup> GetAll(string keyword)
		{
			if (!String.IsNullOrEmpty(keyword))
				return _menuGroupRepository.GetMulti(x => x.Name.Contains(keyword));
			else
				return _menuGroupRepository.GetAll();
		}

		public IEnumerable<MenuGroup> GetAll()
		{
			return _menuGroupRepository.GetAll();
		}

        public IEnumerable<MenuGroup> GetMenuGroupActive()
        {
            return _menuGroupRepository.GetMulti(x => x.Status);
        }
    }
}
