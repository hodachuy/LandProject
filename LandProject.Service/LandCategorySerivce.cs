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
    public interface ILandCategoryService
    {
        IEnumerable<LandCategory> GetAll();
        IEnumerable<LandCategory> GetAllByCondition(string condition, int lTypeID);
        IEnumerable<LandCategory> GetByLandTypeID(int lTypeID);
        LandCategory Add(LandCategory landCategory);
        LandCategory Delete(int id);
        void Update(LandCategory LandCategory);
        void Save();
    }
    public class LandCategoryService : ILandCategoryService
    {
        ILandCategoryRepository _landCategoryRepository;
        IUnitOfWork _unitOfWork;
        public LandCategoryService(ILandCategoryRepository landCategoryRepository, IUnitOfWork unitOfWork)
        {
            _landCategoryRepository = landCategoryRepository;
            _unitOfWork = unitOfWork;
        }
        public LandCategory Add(LandCategory landCategory)
        {
            return _landCategoryRepository.Add(landCategory);
        }

        public LandCategory Delete(int id)
        {
            return _landCategoryRepository.Delete(id);
        }

        public IEnumerable<LandCategory> GetAll()
        {
            return _landCategoryRepository.GetAll(new string[] { "LandType" });
        }

        public IEnumerable<LandCategory> GetAllByCondition(string condition, int lTypeID)
        {
            if (!String.IsNullOrEmpty(condition) && lTypeID == 0)
                return _landCategoryRepository.GetMulti(x => x.Name.Contains(condition), new string[] { "LandType" });
            if (String.IsNullOrEmpty(condition) && lTypeID != 0)
                return _landCategoryRepository.GetMulti(x => x.LandTypeID == lTypeID, new string[] { "LandType" });
            if (!String.IsNullOrEmpty(condition) && lTypeID != 0)
                return _landCategoryRepository.GetMulti(x => x.Name.Contains(condition) && x.LandTypeID == lTypeID, new string[] { "LandType" });
            return _landCategoryRepository.GetAll(new string[] { "LandType" });
        }

        public IEnumerable<LandCategory> GetByLandTypeID(int lTypeID)
        {
            return _landCategoryRepository.GetMulti(x => x.LandTypeID == lTypeID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(LandCategory LandCategory)
        {
            _landCategoryRepository.Update(LandCategory);
        }
    }
}
