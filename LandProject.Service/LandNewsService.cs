using LandProject.Common;
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
    public interface ILandNewsService
    {
        IEnumerable<LandNews> GetAll();
        IEnumerable<LandNewsFilterViewModel> GetAllByFilter(string filter, string sort, int page, int pageSize);
        LandNewsFilterViewModel GetByID(int id);
        LandNews GetLandNewsDbModelByID(int id);
        LandNews Add(LandNews landNews);
        LandNews Delete(int id);
        void PublishedLandNews(LandNews landNews);
        void Update(LandNews landNews);
        void Save();
    }
    public class LandNewsService : ILandNewsService
    {
        ILandNewsRepository _landNewsRepository;
        IUnitOfWork _unitOfWork;
        public LandNewsService(ILandNewsRepository landNewsRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _landNewsRepository = landNewsRepository;
        }
        public LandNews Add(LandNews landNews)
        {
            return _landNewsRepository.Add(landNews);
        }

        public LandNews Delete(int id)
        {
            return _landNewsRepository.Delete(id);
        }

        public IEnumerable<LandNews> GetAll()
        {
            return _landNewsRepository.GetAll();
        }

        public IEnumerable<LandNewsFilterViewModel> GetAllByFilter(string filter, string sort, int page, int pageSize)
        {
            return _landNewsRepository.GetLandNewsByFilter(filter,sort,page,pageSize);
        }

        public LandNewsFilterViewModel GetByID(int id)
        {
            return _landNewsRepository.GetLandNewsByID(id);
        }

        public LandNews GetLandNewsDbModelByID(int id)
        {
            return _landNewsRepository.GetSingleById(id);
        }

        public void PublishedLandNews(LandNews landNews)
        {
            _landNewsRepository.PublishLandNews(landNews);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(LandNews landNews)
        {
            _landNewsRepository.Update(landNews);
        }
    }
}
