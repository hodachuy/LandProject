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
	public interface IMenuService
	{
		IEnumerable<Menu> GetMenu();
		IEnumerable<Menu> GetMenuByMenuGroup(int id);
        IEnumerable<Menu> GetMenuActiveByMenuGroup(int id);
        IEnumerable<Menu> GetAll();
		IEnumerable<Menu> GetAll(string keyword);
		IEnumerable<Menu> GetAll(int menuGroupID,string keyword);
		Menu Add(Menu menu);
		void Update(Menu menu);
		Menu Delete(int id);

		Menu GetByIdMenu(int id);
		IEnumerable<Menu> GetAllByParentId(int parentId);
		void Save();
	}
	public class MenuService : IMenuService
	{
		IMenuRepository _menuRepository;

		IUnitOfWork _unitOfWork;
		public MenuService(IMenuRepository menuRepository, IUnitOfWork unitOfWork)
		{
			_menuRepository = menuRepository;
			_unitOfWork = unitOfWork;
		}

		public IEnumerable<Menu> GetMenu()
		{
			return _menuRepository.GetMulti(x => x.Status);
		}

		public Menu Add(Menu menu)
		{
			return _menuRepository.Add(menu);
		}

		public void Update(Menu menu)
		{
			_menuRepository.Update(menu);
		}

		public Menu Delete(int id)
		{
			return _menuRepository.Delete(id);
		}

		public Menu GetByIdMenu(int id)
		{
			return _menuRepository.GetSingleById(id);
		}
		public void Save()
		{
			_unitOfWork.Commit();
		}

		public IEnumerable<Menu> GetAll(string keyword)
		{
			if (!String.IsNullOrEmpty(keyword))
				return _menuRepository.GetMulti(x => x.Name.Contains(keyword));
			else
				return _menuRepository.GetAll();
		}

		public IEnumerable<Menu> GetAllByParentId(int parentId)
		{
			return _menuRepository.GetMulti(x => x.Status && x.ParentID == parentId);
		}

		public IEnumerable<Menu> GetAll()
		{
			return _menuRepository.GetAll();
		}

		public IEnumerable<Menu> GetMenuByMenuGroup(int id)
		{
			return _menuRepository.GetMulti(x=>x.MenuGroupID == id);
		}

        public IEnumerable<Menu> GetMenuActiveByMenuGroup(int id)
        {
            return _menuRepository.GetMulti(x => x.MenuGroupID == id && x.Status);
        }

		public IEnumerable<Menu> GetAll(int menuGroupID, string keyword)
		{
			var lstMenu = _menuRepository.GetAll(new string[] { "MenuGroup" });
			if (!String.IsNullOrEmpty(keyword))
				lstMenu = lstMenu.Where(x => x.Name.Contains(keyword));
			if (menuGroupID != 0)
				lstMenu = lstMenu.Where(x => x.MenuGroupID == menuGroupID);
			return lstMenu;
		}
	}
}
