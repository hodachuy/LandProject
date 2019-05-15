using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LandProject.Data.Infrastructure;
using LandProject.Data.Repositories;
using LandProject.Model.Models;


namespace LandProject.Service
{
    public interface IPageService
    {
        Page GetByAlias(string alias);
        IEnumerable<Page> GetAll(string keyword);
        IEnumerable<Page> GetPage();
        Page Add(Page page);
        void Update(Page page);
        Page Delete(int id);
        Page GetByIdPage(int id);
        void Save();
    }
    public class PageService : IPageService
    {
        IPageRepository _pageRepository;
        IUnitOfWork _unitOfWork;
        public PageService(IPageRepository pageRepository, IUnitOfWork unitOfWork)
        {
            this._pageRepository = pageRepository;
            this._unitOfWork = unitOfWork;
        }

        public Page Add(Page page)
        {
            return _pageRepository.Add(page);
        }

        public Page Delete(int id)
        {
            return _pageRepository.Delete(id);
        }

        public IEnumerable<Page> GetAll(string keyword)
        {
            if (!String.IsNullOrEmpty(keyword))
                return _pageRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                return _pageRepository.GetAll();
        }
        public Page GetByAlias(string alias)
        {
            return _pageRepository.GetSingleByCondition(x => x.Alias == alias);
        }

        public Page GetByIdPage(int id)
        {
            return _pageRepository.GetSingleById(id);
        }

        public IEnumerable<Page> GetPage()
        {
            return _pageRepository.GetMulti(x => x.Status);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Page page)
        {
            _pageRepository.Update(page);
        }
    }
}
